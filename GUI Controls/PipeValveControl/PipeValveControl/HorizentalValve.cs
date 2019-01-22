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
    public partial class HorizentalValve : PipeControlBase
    {
        public HorizentalValve()
        {
            InitializeComponent();
            this.Images = imageList;

            HasUpNode = false;
            HasDownNode = false;
            HasLeftNode = true;
            HasRightNode = true;
        }

    }
}
