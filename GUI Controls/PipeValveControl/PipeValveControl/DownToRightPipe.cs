﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PipeValveControl
{
    public partial class DownToRightPipe : PipeControlBase
    {
        public DownToRightPipe()
        {
            InitializeComponent();
            this.Images = imageList;

            HasUpNode = false;
            HasDownNode = true;
            HasLeftNode = false;
            HasRightNode = true;
        }
    }
}
