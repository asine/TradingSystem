//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Oybab.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Printlog
    {
        public long PrintLogId { get; set; }
        public long OperateId { get; set; }
        public string OperateName { get; set; }
        public long PrintVersion { get; set; }
        public string Model { get; set; }
        public Nullable<long> AdminId { get; set; }
        public Nullable<long> DeviceId { get; set; }
        public Nullable<long> Remark { get; set; }
        public long AddTime { get; set; }
    }
}