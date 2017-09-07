using System;

namespace ASTRA.EMSG.Business.Entities.Common
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public virtual DateTime? CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? UpdatedAt { get; set; }

        public virtual string UpdatedBy { get; set; }

        public virtual Guid Id { get; set; }

        public virtual bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id.Equals(Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is Entity)) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}