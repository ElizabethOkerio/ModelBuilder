﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;

namespace Microsoft.OData.ModelBuilder.Tests.TestModels
{
    public class EnumModel
    {
        public int Id { get; set; }
        public SimpleEnum Simple { get; set; }
        public SimpleEnum? SimpleNullable { get; set; }
        public LongEnum Long { get; set; }
        public ByteEnum Byte { get; set; }
        public SByteEnum SByte { get; set; }
        public ShortEnum Short { get; set; }
        public UShortEnum UShort { get; set; }
        public UIntEnum UInt { get; set; }
        public FlagsEnum Flag { get; set; }
        public FlagsEnum? FlagNullable { get; set; }
    }

    public enum UShortEnum : ushort
    {
        FirstUShort,
        SecondUShort,
        ThirdUShort
    }

    public enum UIntEnum : uint
    {
        FirstUInt,
        SecondUInt,
        ThirdUInt
    }

    public enum SByteEnum : sbyte
    {
        FirstSByte,
        SecondSByte,
        ThirdSByte
    }

    public enum ByteEnum : byte
    {
        FirstByte,
        SecondByte,
        ThirdByte
    }

    public enum SimpleEnum
    {
        First,
        Second,
        Third,
        Fourth
    }

    public enum ShortEnum : short
    {
        FirstShort,
        SecondShort,
        ThirdShort
    }

    public enum LongEnum : long
    {
        FirstLong,
        SecondLong,
        ThirdLong,
        FourthLong
    }

    [Flags]
    public enum FlagsEnum
    {
        One = 0x1,
        Two = 0x2,
        Four = 0x4
    }
}
