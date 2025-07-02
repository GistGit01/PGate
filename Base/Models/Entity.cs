using FreeSql;
using FreeSql.DataAnnotations;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    public abstract class Entity<TEntity> : BaseEntity<TEntity, long> where TEntity : class
    {
        public Entity() : base()
        {
            this.Id = new IdWorker(1, 1).NextId();
            this.CreateDateTime = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        public TEntity Clone()
        {
            return (TEntity)this.MemberwiseClone();
        }

        public void LogicalDelete()
        {
            if (IsDeleted)
                throw new Exception("已是删除状态");
            if (!IsDeleted)
                IsDeleted = true;
        }

        [Column(IsPrimary = true, IsIdentity = false)]
        public override long Id { get; set; }

        public DateTime CreateDateTime { get; private set; }

        public bool IsDeleted { get; private set; } = false;
    }
}
