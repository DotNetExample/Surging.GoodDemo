﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LZN.Core
{

    public interface IEntity : IEntity<string>
    {

    }

   public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { set; get; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="Id"/>).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();

        int IsDelete { set; get; }

        DateTime CreateDate { set; get; }
    }
}
