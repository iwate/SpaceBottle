﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CalculationWorkerRole
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class spacebottle_dbEntities : DbContext
    {
        public spacebottle_dbEntities()
            : base("name=spacebottle_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Destination> Destination { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Satellite> Satellite { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TodoItem> TodoItem { get; set; }
    }
}
