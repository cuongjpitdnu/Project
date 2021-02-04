using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Services.Navigation;
using GP40Main.Models;
using GP40Tree;
using System.Collections;
using GP40Common;
using SkiaSharp.Views.Desktop;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views
{
    public partial class DemoUserControl : BaseUserControl
    {
        private SKControl skTree;
        private clsTreeDraw objFTree;
        Hashtable hasMember;
        private int intMemberCount = 0;
        private int intMaxFLevelCount = 3;
        private clsConst.ENUM_MEMBER_TEMPLATE enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;

        public DemoUserControl(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            var list = new List<Employee>()
            {
                new Employee { Id = 1, Name = "Joe", Age = 18},
                new Employee { Id = 2, Name = "Jodsde2", Age = 18 },
                new Employee { Id = 3, Name = "Joedsds2", Age = 18 },
                new Employee { Id = 4, Name = "Joess2", Age = 18 },
                new Employee { Id = 5, Name = "Joess2", Age = 25 },
            };

            var bindingList = new BindingList<Employee>(list);
            var source = new BindingSource(bindingList, null);
            metroGrid1.AutoGenerateColumns = false;
            metroGrid1.DataSource = source;

            objFTree = new clsTreeDraw();
            skTree = objFTree.TreeDraw;
            hasMember = objFTree.Family;
            //panelMiddle.Controls.Add(skTree);
        }
    }
}
