using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ResultBattern
{
    public record Result(bool IsSuccess , string? Erorr = default , ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result Failed(string ErorrMessage) => new Result(false , ErorrMessage , ResultKind.Conflict);
        public static Result NotFound(string ErorrMessage = "Not Found") => new Result(false , ErorrMessage , ResultKind.NotFound);
        public static Result ValidationFailed(string ErorrMessage) => new Result(false , ErorrMessage , ResultKind.ValidationFailed);
    }




    public record Result<TValue>(bool IsSuccess , TValue? Value  , string? Erorr = default , ResultKind Kind = ResultKind.Ok)
    {
        public static Result<TValue> Ok(TValue value) => new (true , value);
        public static Result<TValue> Failed(string ErorrMessage) => new (false , default ,  ErorrMessage , ResultKind.Conflict);
        public static Result<TValue> NotFound(string ErorrMessage = "Not Found") => new (false , default , ErorrMessage , ResultKind.NotFound);
        public static Result<TValue> ValidationFailed(string ErorrMessage) => new (false , default, ErorrMessage , ResultKind.ValidationFailed);
    }


}
