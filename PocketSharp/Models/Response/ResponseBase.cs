﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PocketSharp.Models
{
  [DataContract]
  class ResponseBase
  {
    [DataMember(Name = "status")]
    public bool Status { get; set; }
  }
}