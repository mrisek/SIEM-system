using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Windows_Forms_Application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // Obavezna metoda za Designer support
            InitializeComponent();

            ApplicationTextBox.Text = DisplayEventLogProperties("Application");
            SecurityTextBox.Text = DisplayEventLogProperties("Security");
            SystemTextBox.Text = DisplayEventLogProperties("System");
            OperationsTextBox.Text = DisplayEventLogProperties("Operations Manager");
        }

        #region Private variables
        
        private Process[] procesi;  //lista aktivnih procesa
        private string putanja = ""; //putanja procesa kojeg želimo otvoriti
        private Process proc = new Process();   //novi proces kojeg stvaramo
        private ArrayList arrayProcesa = new ArrayList();   //array sastavljen od otvorenih procesa
        private List<Process> listaProc;    //sortirana lista svih aktivnih procesa

        #endregion

        #region Private methods

        static string DisplayEventLogProperties(string logDisplayName)
        {
            // Iterate through the current set of event log files,
            // displaying the property settings for each file.
            EventLog[] eventLogs = EventLog.GetEventLogs();
            foreach (EventLog e in eventLogs.Where(n => n.LogDisplayName == logDisplayName))
            {
                StringBuilder properties = new StringBuilder();
                Int64 sizeKB = 0;

                properties.AppendFormat("{0} log details:{1}{1}", e.LogDisplayName, Environment.NewLine);
                properties.AppendFormat("Log name = {0}{1}", e.Log, Environment.NewLine);

                properties.AppendFormat("Number of event log entries = {0}{1}", e.Entries.Count.ToString(), Environment.NewLine);

                // Determine if there is an event log file for this event log.
                RegistryKey regEventLog = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\EventLog\\" + e.Log);
                if (regEventLog != null)
                {
                    Object temp = regEventLog.GetValue("File");
                    if (temp != null)
                    {
                        properties.AppendFormat("Log file path = {0}{1}", temp.ToString(), Environment.NewLine);
                        FileInfo file = new FileInfo(temp.ToString());

                        // Get the current size of the event log file.
                        if (file.Exists)
                        {
                            sizeKB = file.Length / 1024;
                            if ((file.Length % 1024) != 0)
                            {
                                sizeKB++;
                            }
                            properties.AppendFormat("Current size = {0} kilobytes{1}", sizeKB.ToString(), Environment.NewLine);
                        }
                    }
                    else
                    {
                        properties.AppendFormat("Log file path = <not set>{0}", Environment.NewLine);
                    }

                        // Display the maximum size and overflow settings.

                        sizeKB = e.MaximumKilobytes;
                        properties.AppendFormat("Maximum size = {0} kilobytes{1}", sizeKB.ToString(), Environment.NewLine);
                        properties.AppendFormat("Overflow setting = {0}{1}", e.OverflowAction.ToString(), Environment.NewLine);
                        properties.AppendFormat("EnableRaisingEvents = {0}{1}{1}", e.EnableRaisingEvents.ToString(), Environment.NewLine);

                    switch (e.OverflowAction)
                    {
                        case OverflowAction.OverwriteOlder:
                            properties.AppendFormat("Entries are retained a minimum of {0} days.{1}",
                                e.MinimumRetentionDays,
                                Environment.NewLine);
                            break;
                        case OverflowAction.DoNotOverwrite:
                            properties.AppendFormat("Older entries are not overwritten.{0}",
                                Environment.NewLine);
                            break;
                        case OverflowAction.OverwriteAsNeeded:
                            properties.AppendFormat("If number of entries equals max size limit, a new event log entry overwrites the oldest entry.{0}",
                                Environment.NewLine);
                            break;
                        default:
                            break;
                    }
                }

                return properties.ToString();
            }

            return "";
        }

        /// <summary>
        /// Privatna metoda za kreiranje sortirane liste aktivnih procesa
        /// </summary>
        private void activeProcessSortedList()
        {
            int i = 0;

            procesi = Process.GetProcesses();

            listaProc = procesi.OrderBy(x => x.ProcessName).ToList();

            foreach (Process p in listaProc)
            {
                listBox1.Items.Add(p.ProcessName);
                i++;
            }

            brojAktivnihProcesaLabel.Text = i.ToString();

        }

        /// <summary>
        /// Privatna metoda koja se pokreće svaki put pri loadu forme
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            activeProcessSortedList();
        }
       
        /// <summary>
        /// Tipka "Prekini sve označene procese"
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void prekiniProcesButton_Click(object sender, EventArgs e)
        {
            try
            {
                //procesi[listBox1.SelectedIndex].Kill();

                int ID = listaProc[listBox1.SelectedIndex].Id;
                Process p = Process.GetProcessById(ID);
                p.Kill();

                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                karakteristikeProcesaTextBox.Text = "Odabrani proces je uspješno prekinut!";
            }
            catch (Exception ex)
            {
                karakteristikeProcesaTextBox.Text = "Nije moguće prekinuti odabrani proces."
                    + Environment.NewLine + Environment.NewLine
                    + ex.Message;
            }
        }

        #endregion

        #region NOVI PROCES

        /// <summary>
        /// Tipka "Otvori proces" za pokretanje novog procesa
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void otvoriProcesButton_Click(object sender, EventArgs e)
        {
            if (noviProcesOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                putanja = noviProcesOpenFileDialog.FileName;
                textBox1.Text = putanja;

                proc.StartInfo.FileName = putanja;
                proc.Start();
                arrayProcesa.Add(proc);
            }
            else
            {
                textBox1.Text = "Novi proces nije odabran.";
            }
        }


        //tipka "Zatvori proces"
        private void zatvoriProcesButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Process p in arrayProcesa)
                {
                    p.CloseMainWindow();
                }
            }
            catch 
            {
                textBox1.Text = "Proces je već zatvoren";
                arrayProcesa.Clear();
            }
        }

        /// <summary>
        /// Nakon što unesemo ime procesa možemo ga otvoriti tipkom "Otvori prema imenu procesa"
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void otvoriPremaImenuProcesaButton_Click(object sender, EventArgs e)
        {
            try
            {
                proc.StartInfo.FileName = otvoriPremaImenuProcesaTextBox.Text;
                proc.Start();
                arrayProcesa.Add(proc);
                otvoriPremaImenuProcesaTextBox.Clear();
            }
            catch
            {
                otvoriPremaImenuProcesaTextBox.Text = "Upišite ispravan naziv procesa.";
            }
        }

        /// <summary>
        /// Za brže otvaranje novog procesa pritiskom na tipku Enter
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void otvoriPremaImenuProcesaTextBox_TextChanged(object sender, EventArgs e)
        {
            //TODO: Nije moguće napraviti prelaz u novi red zbog ovog eventa
            //this.AcceptButton = otvoriPremaImenuProcesaButton;
        }

        #endregion

        #region TASK MANAGER

        private void dohvatiKarakteristikeProcesa() 
        {
            try
            {
                if (listaProc[listBox1.SelectedIndex].ProcessName != "Idle")
                {

                    karakteristikeProcesaTextBox.Clear();

                    karakteristikeProcesaTextBox.Text += "Process Name: " + listaProc[listBox1.SelectedIndex].ProcessName + Environment.NewLine;

                    if (listaProc[listBox1.SelectedIndex].Responding)
                    {
                        karakteristikeProcesaTextBox.Text += "Status = Running" + Environment.NewLine;
                    }
                    else
                    {
                        karakteristikeProcesaTextBox.Text += "Status = Not Responding" + Environment.NewLine;
                    }

                    // Define variables to track the peak 
                    // memory usage of the process. 
                    long peakPagedMem = 0;
                    long peakWorkingSet = 0;
                    long peakVirtualMem = 0;

                    karakteristikeProcesaTextBox.Text += Environment.NewLine + "Handle count: " + listaProc[listBox1.SelectedIndex].HandleCount.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Base priority: " + listaProc[listBox1.SelectedIndex].BasePriority.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Priority class: " + listaProc[listBox1.SelectedIndex].PriorityClass.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "User processor time: " + listaProc[listBox1.SelectedIndex].UserProcessorTime.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Privileged processor time: " + listaProc[listBox1.SelectedIndex].PrivilegedProcessorTime.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Total processor time: " + listaProc[listBox1.SelectedIndex].TotalProcessorTime.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += Environment.NewLine + "Paged system memory size: " + listaProc[listBox1.SelectedIndex].PagedSystemMemorySize.ToString() + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Paged memory size 64: " + listaProc[listBox1.SelectedIndex].PagedMemorySize64.ToString() + Environment.NewLine;


                    peakPagedMem = listaProc[listBox1.SelectedIndex].PeakPagedMemorySize64;
                    peakVirtualMem = listaProc[listBox1.SelectedIndex].PeakVirtualMemorySize64;
                    peakWorkingSet = listaProc[listBox1.SelectedIndex].PeakWorkingSet64;

                    // Display peak memory statistics for the process.
                    karakteristikeProcesaTextBox.Text += Environment.NewLine + "Peak memory statistics:" + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Peak physical memory usage of the process: " + peakWorkingSet + Environment.NewLine
                        + "Peak paged memory usage of the process: " + peakPagedMem + Environment.NewLine
                        + "Peak virtual memory usage of the process : " + peakVirtualMem + Environment.NewLine;


                    karakteristikeProcesaTextBox.Text += Environment.NewLine + "Current Process ID: " + listaProc[listBox1.SelectedIndex].Id + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Start time: " + listaProc[listBox1.SelectedIndex].StartTime + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Max working set " + listaProc[listBox1.SelectedIndex].MaxWorkingSet + Environment.NewLine;

                    karakteristikeProcesaTextBox.Text += "Min working set: " + listaProc[listBox1.SelectedIndex].MinWorkingSet;
                }
            }
            catch
            {
                karakteristikeProcesaTextBox.Text = "Nije raspoloživo.";
            }
        }

        private void zatvoriProcesToolStripMenuItem_Click(object sender, EventArgs e)
        //nakon desnog klika na prvi textBox
        //prekidam samo jedan odabrani proces
        {
            try
            {
                procesi[listBox1.SelectedIndex].Kill();
            }
            catch(Exception ex)
            {
                karakteristikeProcesaTextBox.Text = "Nije moguće prekinuti odabrani proces./n/n" + ex.Message;
            }
        }
        //ili dobivam njegove karakteristike
        //pomoću kontekstnog menija
        private void karakteristikeProcesaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dohvatiKarakteristikeProcesa();
        }

        #endregion

        #region TEKSTUALNE DATOTEKE

        //tipka "New"
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        //tipka "Open"
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Log files(*.log)|*.log|Txt files(*.txt)|*.txt|All Files(*.*)|*.*";

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog2.FileName, RichTextBoxStreamType.PlainText);
            }
            this.Text = openFileDialog2.FileName;
        }

        //tipka "Save As"
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Log files(*.log)|*.log|Text Document(*.txt)|*.txt|All Files(*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
            this.Text = saveFileDialog1.FileName;
        }

        //tipka "Cut"
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        //tipka "Copy"
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        //tipka "Paste"
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        //tipka "Undo"
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        //tipka "Redo"
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        //tipka "Font"
        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.SelectionFont;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionFont = fontDialog1.Font;
            }
        }

        //tipka "Background Color"
        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = colorDialog1.Color;
            }
        }

        //Tipka "About"
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by:\nMatija Risek\n04.11.2016.", "About this application", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dohvatiKarakteristikeProcesa();
        }

        private void osvjeziButton_Click(object sender, EventArgs e)
        {
            Array.Clear(procesi, 0, procesi.Length);
            listBox1.Items.Clear();
            activeProcessSortedList();
        }

    }
}
