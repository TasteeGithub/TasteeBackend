using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace Tastee.Infrastucture.Data.Context
{
    public partial class UserTokens: Entity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Uid { get; set; }
        public bool IsActive { get; set; }
    }
}
