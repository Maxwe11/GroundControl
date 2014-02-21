﻿namespace GroundControl.Common.Mapping.Parameters
{
    using System;
    using System.Runtime.Serialization;

    using GroundControl.Common.Extensions;
    using GroundControl.Common.Mapping.Converters;
    using GroundControl.Common.Mapping.Visitors;
    using GroundControl.Common.Properties;

    [DataContract]
    [KnownType(typeof(Integer8))]
    [KnownType(typeof(Integer16))]
    [KnownType(typeof(Integer32))]
    [KnownType(typeof(UnsignedInteger8))]
    [KnownType(typeof(UnsignedInteger16))]
    [KnownType(typeof(UnsignedInteger32))]
    public abstract class Parameter
    {
        #region Fields

        private Converter mConverter;

        #endregion

        #region Constructor

        protected Parameter(string name, string displayName, int bitsCount, int maxBitsCount)
        {
            name.CheckNull("name");
            displayName.CheckNull("displayName");

            if (bitsCount < 1 || bitsCount > maxBitsCount)
            {
                var msg = "bitsCount should be in inclusive range [1, " + maxBitsCount + "]";
                throw new ArgumentException(msg, "bitsCount");
            }

            Name = name;
            DisplayName = displayName;
            BitsCount = bitsCount;
        }

        #endregion

        #region Properties

        #region Serialized

        [DataMember(IsRequired = true, Order = 0)]
        public string Name { get; private set; }

        [DataMember(IsRequired = true, Order = 1)]
        public string DisplayName { get; private set; }

        [DataMember(IsRequired = true, Order = 2)]
        public int BitsCount { get; private set; }

        [DataMember(IsRequired = false,  Order = 3, EmitDefaultValue = false)]
        public string ConverterName { get; private set; }

        #endregion

        #region NonSerialized

        public Converter Converter { get; private set; }

        public bool HasConverter { get { return Converter != null; } }

        public abstract Type Type { get; }

        public abstract object Value { get; }

        #endregion

        #endregion

        #region Methods

        [OnDeserialized]
        [UsedImplicitly]
        void OnDeserialized(StreamingContext c)
        {
//            if (ConverterName == null)
//                Converter = DefaultConverter
//            else
//                Converter = Converters[Convertername]

            if (DisplayName == null)
                DisplayName = Name;
        }

        public abstract void Apply(IParameterVisitor visitor);

        #endregion
    }
}
