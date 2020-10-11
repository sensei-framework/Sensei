using System;

namespace Sensei.AspNet.DbProcessor
{
    /// <summary>
    /// Flags for identify then entity state when to run a db processor attribute
    /// </summary>
    [Flags]
    public enum DbEntityState
    {
        /// <summary>
        /// The entity was added
        /// </summary>
        Added = 1,
        
        /// <summary>
        /// The entity was modified
        /// </summary>
        Modified = 2,
        
        /// <summary>
        /// The entity was deleted
        /// </summary>
        Deleted = 4
    }
}