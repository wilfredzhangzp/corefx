﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using Xunit;

namespace System.ComponentModel.Tests
{
    public class MyTypeDescriptorContext : ITypeDescriptorContext
    {
        public IContainer Container { get { return null; } }
        public object Instance { get { return null; } }
        public PropertyDescriptor PropertyDescriptor { get { return null; } }
        public bool OnComponentChanging() { return true; }
        public void OnComponentChanged() { }
        public object GetService(Type serviceType) { return null; }
    }

    public struct SomeValueType
    {
        public int a;
    }

    public enum SomeEnum
    {
        Add,
        Sub,
        Mul
    }

    [Flags]
    public enum SomeFlagsEnum
    {
        Option1 = 1,
        Option2 = 2,
        Option3 = 4
    }

    public class FormattableClass : IFormattable
    {
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return FormattableClass.Token;
        }
        public const string Token = "Formatted class.";
    }

    public class Collection1 : ICollection
    {
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class MyTypeListConverter : TypeListConverter
    {
        public MyTypeListConverter(Type[] types)
            : base(types)
        {
        }
    }

    [TypeConverter("System.ComponentModel.Tests.BaseClassConverter, System.ComponentModel.TypeConverter.Tests, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public class BaseClass
    {
        public BaseClass()
        {
            BaseProperty = 1;
        }
        public override bool Equals(object other)
        {
            BaseClass otherBaseClass = other as BaseClass;
            if (otherBaseClass == null)
            {
                return false;
            }
            if (otherBaseClass.BaseProperty == this.BaseProperty)
            {
                return true;
            }
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public int BaseProperty;
    }

    public class BaseClassConverter : TypeConverter
    {
        public BaseClassConverter(string someString) { throw new InvalidOperationException("This constructor should not be invoked by TypeDescriptor.GetConverter."); }
        public BaseClassConverter() { }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(int))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is int)
            {
                BaseClass baseClass = new BaseClass();
                baseClass.BaseProperty = (int)value;
                return baseClass;
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                BaseClass baseClass = value as BaseClass;
                if (baseClass != null)
                {
                    return baseClass.BaseProperty;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter("System.ComponentModel.Tests.DerivedClassConverter")]
    internal class DerivedClass : BaseClass
    {
        public DerivedClass()
            : base()
        {
            this.DerivedProperty = 2;
        }
        public DerivedClass(int i)
            : base()
        {
            this.DerivedProperty = i;
        }
        public override bool Equals(object other)
        {
            DerivedClass otherDerivedClass = other as DerivedClass;
            if (otherDerivedClass == null)
            {
                return false;
            }
            if (otherDerivedClass.DerivedProperty != this.DerivedProperty)
            {
                return false;
            }
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public int DerivedProperty;
    }

    internal class DerivedClassConverter : TypeConverter
    {
        public DerivedClassConverter() { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(int))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is int)
            {
                DerivedClass derived = new DerivedClass();
                derived.BaseProperty = (int)value;
                derived.DerivedProperty = (int)value;
                return derived;
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                DerivedClass derived = value as DerivedClass;
                if (derived != null)
                {
                    return derived.BaseProperty + derived.DerivedProperty;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(IBaseConverter))]
    public interface IBase
    {
        int InterfaceProperty { get; set; }
    }

    public interface IDerived : IBase
    {
        int DerivedInterfaceProperty { get; set; }
    }

    public class ClassIBase : IBase
    {
        public ClassIBase()
        {
            this.InterfaceProperty = 10;
        }
        public int InterfaceProperty { get; set; }
    }

    public class ClassIDerived : IDerived
    {
        public ClassIDerived()
        {
            this.InterfaceProperty = 20;
            this.DerivedInterfaceProperty = this.InterfaceProperty / 2;
        }
        public int InterfaceProperty { get; set; }
        public int DerivedInterfaceProperty { get; set; }
    }

    public class IBaseConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(int))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                IBase baseInterface = (IBase)value;
                return "InterfaceProperty = " + baseInterface.InterfaceProperty.ToString();
            }
            if (destinationType == typeof(int))
            {
                IBase baseInterface = (IBase)value;
                return baseInterface.InterfaceProperty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter("System.ComponentModel.Tests.InvalidConverter")]
    internal class ClassWithInvalidConverter : BaseClass
    {
    }

    public class InvalidConverter : TypeConverter
    {
        public InvalidConverter(string someString)
        {
            throw new InvalidOperationException("This constructor should not be invoked by TypeDescriptor.GetConverter.");
        }
        // Default constructor is missing, we expect the following exception when getting a converter:
        // System.MissingMethodException: No parameterless constructor defined for this object.
    }

    // TypeDescriptor should default to the TypeConverter in this case.
    public class ClassWithNoConverter
    {
    }
}
