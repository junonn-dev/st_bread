using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmSerialSetting : Form
    {
        string[] sBaudRate = new string[] { "4800", "9600", "14400", "19200", "28800", "38400", "56000", "57600", "115200" };
        string[] sDatabit = new string[] { "5", "6", "7", "8" };
        string[] sStopbit = new string[] { "None", "1", "1.5", "2" };
        string[] sHandShake = new string[] { "None", "Software", "Hardware" };
        string[] sParity = new string[] { "even", "mark", "none", "odd", "space" };
        //string[] sVan = new string[] { "0-Kovan","2-SPC"};

        clsEnum.Select_SerialSet serialSet = clsEnum.Select_SerialSet.port;
        Inter_Set frm = null;


        public frmSerialSetting()
        {
            InitializeComponent();
        }

        public frmSerialSetting(Inter_Set frm, clsEnum.Select_SerialSet serialSet)
        {
            InitializeComponent();
            this.frm = frm;
            this.serialSet = serialSet;
        }



        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSerialSetting_Load(object sender, EventArgs e)
        {//생성자에 넘어온 값으로 리스트 표시

            switch (serialSet)
            {
                case clsEnum.Select_SerialSet.port:
                    Show_ComPort();
                    break;
                case clsEnum.Select_SerialSet.speed:
                    Show_Speed();
                    break;
                case clsEnum.Select_SerialSet.parity:
                    Show_Parity();
                    break;
                case clsEnum.Select_SerialSet.databits:
                    Show_Databits();
                    break;
                case clsEnum.Select_SerialSet.stopbits:
                    Show_StopBits();
                    break;
                case clsEnum.Select_SerialSet.handshaking:
                    Show_HandShaking();
                    break;
                case clsEnum.Select_SerialSet.van:
                    Show_VAN();
                    break;

            }

        }


        /// <summary>
        /// 현재 PC의 활성 포트 
        /// </summary>
        private void Show_ComPort()
        {
            foreach (string comport in SerialPort.GetPortNames())
            {
                ListViewItem oItem = new ListViewItem(comport);
                lst_Set.Items.Add(oItem);
            }
        }

        private void Show_Speed()
        {
            foreach (string sSpeed in sBaudRate)
            {
                ListViewItem oItem = new ListViewItem(sSpeed);
                lst_Set.Items.Add(oItem);
            }

        }


        private void Show_Parity()
        {
            foreach (string sPar in sParity)
            {
                ListViewItem oItem = new ListViewItem(sPar);
                lst_Set.Items.Add(oItem);
            }
        }


        private void Show_Databits()
        {
            foreach (string sSpeed in sDatabit)
            {
                ListViewItem oItem = new ListViewItem(sSpeed);
                lst_Set.Items.Add(oItem);
            }

        }
        private void Show_StopBits()
        {
            foreach (string sSpeed in sStopbit)
            {
                ListViewItem oItem = new ListViewItem(sSpeed);
                lst_Set.Items.Add(oItem);
            }
        }
        private void Show_HandShaking()
        {
            foreach (string sSpeed in sHandShake)
            {
                ListViewItem oItem = new ListViewItem(sSpeed);
                lst_Set.Items.Add(oItem);
            }
        }

        private void Show_VAN()
        {   
            var enumlist = Enum.GetValues(typeof(clsEnum.Van_Cmpny))
                .Cast<clsEnum.Van_Cmpny>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                    value
                })
            .OrderBy(item => item.value)
            .ToList();

            foreach (var data in enumlist)
            {
                lst_Set.Items.Add(new ListViewItem()
                {
                    Text = string.Format("{0}", data.Description)
                });
            }
        }



        private void Select_Item()
        {
            if (lst_Set.Items.Count == 0)
                return;
            if (frm == null)
                return;

            if (lst_Set.SelectedItems.Count == 1)
            {
                var item = lst_Set.SelectedItems[0];

                frm.Inter_Set_Setting(item.Text, serialSet);
            }

            this.Close();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            Select_Item();
        }

        private void lst_Set_DoubleClick(object sender, EventArgs e)
        {
            Select_Item();
        }


    }
}
