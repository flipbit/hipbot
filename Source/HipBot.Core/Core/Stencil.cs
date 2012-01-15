using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HipBot.Core
{
    /// <summary>
    /// Stencil is a Basic Inversion of Control Container
    /// </summary>
    public class Stencil
    {
        // Singleton variables
        private static Stencil stencil;
        private static readonly Options defaults = new Options();

        // State
        private readonly List<Interface> interfaces;

        /// <summary>
        /// Gets the Stencil Singleton.
        /// </summary>
        public static Stencil Instance
        {
            get
            {
                if (stencil == null)
                {
                    stencil = new Stencil();
                    stencil.Initilize(Defaults);
                }

                return stencil;
            }
        }

        /// <summary>
        /// Gets the default options used to create Stencil singleton.
        /// </summary>
        public static Options Defaults
        {
            get { return defaults; }
        }

        /// <summary>
        /// Represents an interface in the container
        /// </summary>
        private class Interface : ICloneable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Interface"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            public Interface(Type type)
            {
                Type = type;
            }

            /// <summary>
            /// Gets or sets the type of this interface.
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public Type Type { get; set; }

            /// <summary>
            /// Gets or sets the concrete type of this interface.
            /// </summary>
            /// <value>
            /// The type of the concrete.
            /// </value>
            public Type ConcreteType { get; set; }

            /// <summary>
            /// Gets or sets the value of the concrete type that implements the interface.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is a singleton.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is a singleton; otherwise, <c>false</c>.
            /// </value>
            public bool IsSingleton { get; set; }

            /// <summary>
            /// Gets or sets the order that this interface should be processed when
            /// initialized in a list.  This value is set by the OrderAttribute.
            /// </summary>
            /// <value>
            /// The order.
            /// </value>
            public int Order { get; set; }

            /// <summary>
            /// Determines whether this instance can create the specified type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns>
            ///   <c>true</c> if this instance can create the specified type; otherwise, <c>false</c>.
            /// </returns>
            public bool CanCreate(Type type)
            {
                return Type.IsAssignableFrom(type);
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            /// <returns>
            /// A new object that is a copy of this instance.
            /// </returns>
            public object Clone()
            {
                return new Interface(Type)
                {
                    ConcreteType = ConcreteType,
                    IsSingleton = IsSingleton,
                    Value = Value
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stencil"/> class.
        /// </summary>
        public Stencil()
        {
            interfaces = new List<Interface>();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initilize()
        {
            Initilize(Defaults);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initilize(Options options)
        {
            // Generate interfaces
            CreateInterfaces(options.Assemblies);

            // Set interface concrete types
            CreateInterfaceTypes(options.Assemblies, options.UseSingletons);
        }

        /// <summary>
        /// Creates the interfaces.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        private void CreateInterfaces(IEnumerable<Assembly> assemblies)
        {
            interfaces.Clear();

            foreach (var assembly in assemblies)
            {
                CreateInterfaces(assembly);
            }
        }

        /// <summary>
        /// Creates the interfaces.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private void CreateInterfaces(Assembly assembly)
        {
            var types = assembly.GetTypes();

            // Parse interfaces
            foreach (var type in types)
            {
                if (!type.IsInterface) continue;

                interfaces.Add(new Interface(type));
            }
        }

        private void CreateInterfaceTypes(IEnumerable<Assembly> assemblies, bool useSingletons)
        {
            foreach (var assembly in assemblies)
            {
                CreateInterfaceTypes(assembly, useSingletons);
            }
        }

        private void CreateInterfaceTypes(Assembly assembly, bool useSingletons)
        {
            var types = assembly.GetTypes();

            // Parse concrete types
            foreach (var type in types)
            {
                // Skip interfaces
                if (type.IsInterface) continue;

                // Skip abstract classes
                if (type.IsAbstract) continue;                

                // Check is type implements an interface we've processed
                foreach (var @interface in interfaces)
                {
                    if (!@interface.CanCreate(type)) continue;

                    @interface.IsSingleton = useSingletons;

                    // Assign this concrete type to the interface
                    if (@interface.ConcreteType == null)
                    {
                        @interface.Order = GetOrder(type);
                        @interface.ConcreteType = type;
                    }
                    else
                    {
                        // Allow multiple classes to implement the same interface
                        var clone = (Interface)@interface.Clone();
                        clone.Order = GetOrder(type);
                        clone.ConcreteType = type;

                        @interfaces.Add(clone);
                    }

                    break;
                }
            }
        }

        private static int GetOrder(Type type)
        {
            var result = 0;

            var attributes = type.GetCustomAttributes(typeof(OrderAttribute), true);

            foreach (OrderAttribute attribute in attributes)
            {
                result = attribute.Order;
            }

            return result;
        }

        /// <summary>
        /// Resolves an object of type <see cref="T"/> from this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolves an object of the specified type from this instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return CreateInstance(type);
        }

        public IList<T> ResolveAll<T>()
        {
            return ResolveAll(typeof(T)).Select(@object => (T)@object).ToList();
        }

        public IList<object> ResolveAll(Type type)
        {
            var results = new List<object>();

            foreach (var @interface in interfaces.OrderBy(i => i.Order))
            {
                if (@interface.Type == type)
                {
                    results.Add(CreateInstanceFromInterfaceValue(@interface));
                }
            }

            return results;
        }

        private object CreateInstance(Type type, IList<Type> typeChain = null)
        {
            object result;

            // validate we're not recursively creating a type
            if (typeChain != null && typeChain.Contains(type))
            {
                return null;
            }

            if (type.IsInterface)
            {
                result = CreateInstanceFromInterface(type);
            }
            else
            {
                result = CreateInstanceFromConcrete(type, typeChain);
            }

            return result;
        }

        private object CreateInstanceFromInterface(Type type)
        {
            object result = null;

            foreach (var @interface in interfaces)
            {
                if (@interface.Type != type) continue;

                result = CreateInstanceFromInterfaceValue(@interface);

                break;
            }

            return result;
        }

        private object CreateInstanceFromInterfaceValue(Interface @interface)
        {
            var value = @interface.Value;

            if (value == null)
            {
                var chain = new List<Type> { @interface.Type };

                value = CreateInstanceFromConcrete(@interface.ConcreteType, chain);

                if (@interface.IsSingleton)
                {
                    @interface.Value = value;
                }
            }

            return value;
        }

        private object CreateInstanceFromConcrete(Type type, IList<Type> typeChain = null)
        {
            var result = Activator.CreateInstance(type);                
            
            if (typeChain == null)
            {
                typeChain = new List<Type>();
            }
            typeChain.Add(type);

            var properties = result.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;

                // Set public interfaces
                if (propertyType.IsInterface && propertyType.IsPublic && !propertyType.IsGenericType)
                {
                    var value = CreateInstance(propertyType, typeChain);

                    property.SetValue(result, value, null);
                }

                // Set generic lists
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    // Create a generic list
                    var genericType = propertyType.GetGenericArguments()[0];
                    var listType = typeof(List<>).MakeGenericType(genericType);
                    var list = Activator.CreateInstance(listType);

                    var items = ResolveAll(genericType);

                    foreach (var item in items)
                    {
                        listType.InvokeMember("Add", BindingFlags.InvokeMethod, null, list, new[] { item });
                    }

                    property.SetValue(result, list, null);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Holds the options used to initialize the Stencil container.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        public Options()
        {
            // Default to load types from the current assembly
            Assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };

            // Use property injection & singletons by default
            UsePropertyInjection = true;
            UseSingletons = true;
        }

        /// <summary>
        /// Gets the assemblies used to load the container types from.
        /// </summary>
        public IList<Assembly> Assemblies { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether Stencil should use property injection.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if Stencil should use property injection; otherwise, <c>false</c>.
        /// </value>
        public bool UsePropertyInjection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the container should use singletons.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the container should use singletons; otherwise, <c>false</c>.
        /// </value>
        public bool UseSingletons { get; set; }
    }

    /// <summary>
    /// Attribute to allow the specification of class orders in generic lists
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OrderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAttribute"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        public OrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }
    }
}