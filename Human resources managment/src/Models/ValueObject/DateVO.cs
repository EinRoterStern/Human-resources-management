using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Human_resources_managment.Models.ValueObjectModels
{
    public record DateVO
    {
        private DateVO(DateOnly dateOnly) 
        {
            Date = dateOnly;
        }

        public DateOnly Date {  get; private set; }

        public static Result<DateVO> Create(DateOnly dateOnly)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            if (dateOnly < today)
                return Result.Failure<DateVO>($"Дата не может быть раньше сегодняшней ({today}). Указана дата: {dateOnly}.");

            var obj = new DateVO(dateOnly);
            return Result.Success<DateVO>(obj);
        }
    }
}
