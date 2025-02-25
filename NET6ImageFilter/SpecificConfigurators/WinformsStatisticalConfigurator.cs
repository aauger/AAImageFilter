﻿using FilterDotNet.Filters;
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

namespace NET6ImageFilter.SpecificConfigurators
{
    public partial class WinformsStatisticalConfigurator : Form, IPluginConfigurator<StatisticalFilterConfiguration>
    {
        public WinformsStatisticalConfigurator()
        {
            InitializeComponent();
        }

        public StatisticalFilterConfiguration GetPluginConfiguration()
        {
            var ctls = this.modeGroupBox.Controls.OfType<RadioButton>();
            if (this.ShowDialog() == DialogResult.OK)
            {
                return new()
                {
                    Thresholding = this.thresholdCheckbox.Checked,
                    Threshold = int.Parse(this.thresholdTextBox.Text),
                    BlockMask = BlockMaskFromInteger(int.Parse(this.blockSizeTextBox.Text)),
                    Mode = this.modeGroupBox.Controls.OfType<RadioButton>().First(rb => rb.Checked) switch
                    {
                        { Name: "averageRadioButton" } => StatisticalFilterMode.AVERAGE,
                        { Name: "minRadioButton" } => StatisticalFilterMode.MIN,
                        { Name: "maxRadioButton" } => StatisticalFilterMode.MAX,
                        { Name: "medianRadioButton" } => StatisticalFilterMode.MEDIAN,
                        { Name: "modeRadioButton" } => StatisticalFilterMode.MODE,
                        _ => throw new NotImplementedException()
                    }
                };
            }
            return new();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool[,] BlockMaskFromInteger(int i)
        {
            bool[,] result = new bool[i,i];
            for (int n = 0; n < i; n++)
            {
                for (int j = 0; j < i; j++)
                {
                    result[n, j] = true;
                }
            }
            return result;
        }
    }
}
