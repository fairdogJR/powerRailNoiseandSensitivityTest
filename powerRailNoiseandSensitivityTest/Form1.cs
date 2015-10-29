using Agilent.CommandExpert.ScpiNet.AgInfiniium90000AQX_04_60_0016;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace powerRailNoiseandSensitivityTest
{
    public partial class Form1 : Form
    {
        string instrumentAddress = "TCPIP0::localhost::inst0::INSTR";

        public Form1()
        {
            InitializeComponent();

        }
        private void scopeFrontendVoltagetest1(string setupname, string dutname, string filenamePrecursor)
        {
            string idn = null;
            string dispMessage = "";
            int opc = 0;
            int opc1 = 0;
            int opc2 = 0;
            int opc3 = 0;
            int opc4 = 0;
            int opc5 = 0;
            // In order to use below driver class, you need to reference this assembly : [C:\ProgramData\Agilent\Command Expert\ScpiNetDrivers\AgInfiniium90000AQX_04_60_0016.dll]
            //AgInfiniium90000AQX PCSERNO = new AgInfiniium90000AQX("TCPIP0::WINDOWS-S4LISIR::hislip0::INSTR");
            AgInfiniium90000AQX PCSERNO = new AgInfiniium90000AQX(instrumentAddress);
            //PCSERNO.Transport.DefaultTimeout.Set(10000);
            PCSERNO.SCPI.IDN.Query(out idn);
            PCSERNO.SCPI.RST.Command();
            PCSERNO.SCPI.CLS.Command();
            PCSERNO.SCPI.DISK.LOAD.Command(setupname, null);
            //PCSERNO.SCPI.DISK.LOAD.Command("scopesetup_voltage1.set", null);
            System.Threading.Thread.Sleep(3000);
            // Comment text
            toolStripStatusLabel1.Text = "testing BW Limit None ";
            statusStrip1.Refresh();

            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 100");
            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 1");

            //----------------------------
            PCSERNO.Transport.Command.Invoke(":CHAN1:BWL OFF");
            System.Threading.Thread.Sleep(3000);

            //**********************************************************
            //the escape sequences of the next line are extremely important to get the proper format
            // 
            // especially the \"
            //dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " voltage measurement : analog BW limit=None\" ";
            //

            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " voltage measurement | analog BW limit=None\" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            //PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:SET NONE,\"voltage measurement / analog BW limit=None\" " );
            PCSERNO.SCPI.OPC.Query(out opc);
            PCSERNO.SCPI.SINGle.Command();
            System.Threading.Thread.Sleep(5000);
            PCSERNO.SCPI.OPC.Query(out opc1);
            PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname + filenamePrecursor+"offOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
          //  PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname + "voltageAnBWLOffOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
            System.Threading.Thread.Sleep(3000);
            // limit BW to 200MHz
            toolStripStatusLabel1.Text = "testing BW Limit 200MHz ";
            statusStrip1.Refresh();

            //----------------------------
            PCSERNO.Transport.Command.Invoke(":CHAN1:BWL 200e06");
            System.Threading.Thread.Sleep(3000);
            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " voltage measurement | analog BW limit=200MHz\" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            //PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:SET NONE,\"voltage measurement /analog BW limit=200MHz\"");
            PCSERNO.SCPI.OPC.Query(out opc2);
            PCSERNO.SCPI.SINGle.Command();
            PCSERNO.SCPI.OPC.Query(out opc3);
            System.Threading.Thread.Sleep(5000);
            PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname + "voltageAnBWL200MHzOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
            System.Threading.Thread.Sleep(3000);
            // limit BW to 20MHz
            toolStripStatusLabel1.Text = "testing BW 20MHz ";
            statusStrip1.Refresh();


            //----------------------------
            PCSERNO.Transport.Command.Invoke(":CHAN1:BWL 20e06");
            System.Threading.Thread.Sleep(3000);
            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " voltage measurement | analog BW limit=20MHz\" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            //PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:SET NONE,\"voltage measurement / analog BW limit=20MHz\"");
            PCSERNO.SCPI.OPC.Query(out opc4);
            PCSERNO.SCPI.SINGle.Command();
            System.Threading.Thread.Sleep(5000);
            PCSERNO.SCPI.OPC.Query(out opc5);
            PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname + "voltageAnBWL20MHzOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
            System.Threading.Thread.Sleep(3000);

            toolStripStatusLabel1.Text = "Completed test. Waiting for input. ";
            statusStrip1.Refresh();

            System.Threading.Thread.Sleep(3000);

            PCSERNO.SCPI.CDISplay.Command();

            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 50");
            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 50");
            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" COMPLETED TEST \" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            System.Threading.Thread.Sleep(5000);
            dispMessage = ":DISPlay:BOOKmark1:DELete"; //erase message after 5 seconds
            PCSERNO.Transport.Command.Invoke(@dispMessage);

        }
        private void scopeFrontendNoisetest1(string dutname)
        {
            string idn = null;
            string dispMessage = "";
            int opc = 0;
            int opc1 = 0;
            int opc2 = 0;
            int opc3 = 0;
            int opc4 = 0;
            int opc5 = 0;
            // In order to use below driver class, you need to reference this assembly : [C:\ProgramData\Agilent\Command Expert\ScpiNetDrivers\AgInfiniium90000AQX_04_60_0016.dll]
            //AgInfiniium90000AQX PCSERNO = new AgInfiniium90000AQX("TCPIP0::WINDOWS-S4LISIR::hislip0::INSTR");
            AgInfiniium90000AQX PCSERNO = new AgInfiniium90000AQX(instrumentAddress);

            //PCSERNO.Transport.DefaultTimeout.Set(10000);
            PCSERNO.SCPI.IDN.Query(out idn);
            PCSERNO.SCPI.RST.Command();
            PCSERNO.SCPI.CLS.Command();
            PCSERNO.SCPI.DISK.LOAD.Command("scopesetup_noise1.set", null);
            System.Threading.Thread.Sleep(3000);
            // Comment text
            toolStripStatusLabel1.Text = "testing BW Limit None ";
            statusStrip1.Refresh();

            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 100");
            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 1");
            
            //----------------------------
            PCSERNO.Transport.Command.Invoke(":CHAN1:BWL OFF");
            System.Threading.Thread.Sleep(3000);

            //**********************************************************
            //the escape sequences of the next line are extremely important to get the proper format
            // 
            // especially the \"
            //dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " noise measurement : analog BW limit=None\" ";
            //

            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " noise measurement | analog BW limit=None\" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            //PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:SET NONE,\"noise measurement / analog BW limit=None\" " );
            PCSERNO.SCPI.OPC.Query(out opc);
            PCSERNO.SCPI.SINGle.Command();
            System.Threading.Thread.Sleep(5000);
            PCSERNO.SCPI.OPC.Query(out opc1);
            PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname + "noiseAnBWLOffOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
            System.Threading.Thread.Sleep(3000);
            // limit BW to 200MHz
            toolStripStatusLabel1.Text = "testing BW Limit 200MHz ";
            statusStrip1.Refresh();

            //----------------------------
            PCSERNO.Transport.Command.Invoke(":CHAN1:BWL 200e06");
            System.Threading.Thread.Sleep(3000);
            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " noise measurement | analog BW limit=200MHz\" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            //PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:SET NONE,\"noise measurement /analog BW limit=200MHz\"");
            PCSERNO.SCPI.OPC.Query(out opc2);
            PCSERNO.SCPI.SINGle.Command();
            PCSERNO.SCPI.OPC.Query(out opc3);
            System.Threading.Thread.Sleep(5000);
            PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname + "noiseAnBWL200MHzOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
            System.Threading.Thread.Sleep(3000);
            // limit BW to 20MHz
            toolStripStatusLabel1.Text = "testing BW 20MHz ";
            statusStrip1.Refresh();


            //----------------------------
            PCSERNO.Transport.Command.Invoke(":CHAN1:BWL 20e06");
            System.Threading.Thread.Sleep(3000);
            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" " + dutname + " noise measurement | analog BW limit=20MHz\" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            //PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:SET NONE,\"noise measurement / analog BW limit=20MHz\"");
            PCSERNO.SCPI.OPC.Query(out opc4);
            PCSERNO.SCPI.SINGle.Command();
            System.Threading.Thread.Sleep(5000);
            PCSERNO.SCPI.OPC.Query(out opc5);
            PCSERNO.SCPI.DISK.SAVE.IMAGe.Command(dutname+"noiseAnBWL20MHzOpenScopechan1", "PNG", "SCReen", false, "NORMal", null);
            System.Threading.Thread.Sleep(3000);

            toolStripStatusLabel1.Text = "Completed test. Waiting for input. ";
            statusStrip1.Refresh();

            System.Threading.Thread.Sleep(3000);

            PCSERNO.SCPI.CDISplay.Command();

            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 50");
            PCSERNO.Transport.Command.Invoke(":DISPlay:BOOKmark1:YPOSition 50");
            dispMessage = ":DISPlay:BOOKmark1:SET NONE, \" COMPLETED TEST \" ";
            PCSERNO.Transport.Command.Invoke(@dispMessage);
            System.Threading.Thread.Sleep(5000);
            dispMessage = ":DISPlay:BOOKmark1:DELete"; //erase message after 5 seconds
            PCSERNO.Transport.Command.Invoke(@dispMessage);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text= "Running Test";
            statusStrip1.Refresh();
            scopeFrontendNoisetest1("Scope Input shunted ");// do not use non alphanumeric charachers otherwise will get unable to save file message

        }





        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "entering scope test";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendNoisetest1("Scope plus CoAxe shunted ");// do not use non alphanumeric charachers otherwise will get unable to save file message
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendNoisetest1("N7020A Power Rail Probe shunted ");            // do not use non alphanumeric charachers otherwise will get unable to save file message

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
             toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendVoltagetest1("scopesetup_voltage1.set", "Scope plus CoAxe Sine ","3mVSinevoltageAnBWL");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendVoltagetest1("scopesetup_voltage1.set", "N7020A Power Rail Probe No Offset Sine ", "3mVSinevoltageAnBWL");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendVoltagetest1("scopesetup_voltage2.set", "N7020A Power Rail Probe large Offset Sine ", "3mVSinevoltageAnBWL");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendVoltagetest1("scopesetup_voltage1.set", "Scope plus CoAxe Ramp ", "3mVRampvoltageAnBWL");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendVoltagetest1("scopesetup_voltage1.set", "N7020A Power Rail Probe No Offset Ramp ", "3mVRampvoltageAnBWL");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Running Test";
            statusStrip1.Refresh();
            scopeFrontendVoltagetest1("scopesetup_voltage3.set", "N7020A Power Rail Probe large Offset Ramp ", "3mVRampvoltageAnBWL");
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }



    }
}
