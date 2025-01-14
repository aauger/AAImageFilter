﻿using FilterDotNet;
using FilterDotNet.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NET6ImageFilter.Dialogs
{
    public partial class FilterDialog : Form
    {
        private List<IFilter> _filters;
        public IFilter? SelectedFilter { get; set; }

        public FilterDialog(List<IFilter> filters)
        {
            this._filters = filters;
            this.InitializeComponent();
            this.filterListBox.DataSource = _filters;
            this.filterListBox.DisplayMember = "Name";
            this.filterListBox.Update();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if(this.filterListBox.SelectedIndex != -1)
                this.SelectedFilter = (IFilter?)this.filterListBox.SelectedItem;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
                this.filterListBox.DataSource = _filters
                    .Where(f => string.IsNullOrWhiteSpace(searchTextBox.Text) || f.Name.ToLower().Contains(searchTextBox.Text.ToLower()))
                    .ToList();
                this.filterListBox.Update();
        }
    }
}
