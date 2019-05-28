﻿using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.GarmentPreparings.ValueObjects
{
    public class UnitDepartment : ValueObject
    {
        public UnitDepartment()
        {

        }

        public UnitDepartment(int departmentId, string name, string code)
        {
            Id = departmentId;
            Name = name;
            Code = code;
        }

        public int Id { get; }

        public string Name { get; set; }

        public string Code { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
