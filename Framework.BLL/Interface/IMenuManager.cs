﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;

namespace Framework.BLL
{
    public abstract class IMenuManager : BaseBll
    {
        public abstract EHECD_FunctionMenuDTO AddMenu(EHECD_FunctionMenuDTO dto);        
    }
}
