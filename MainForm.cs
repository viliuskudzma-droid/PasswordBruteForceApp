using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordBruteForceApp
{
    public class MainForm : Form
    {
        Button btnCreatePassword;
        Button btnStartSingle;
        Button btnStartMulti;
        Button btnStop;

        Label lblPassword;
        Label lblHash;
        Label lblResult;
        Label lblTime;
        Label lblThreads;
        Label lblCompare;

        ProgressBar progressBar;

        string createdPassword = "";
        string targetHash = "";

        double singleTime = 0;
        double multiTime = 0;

        CancellationTokenSource cts;

        public MainForm()
        {
            Text = "Password Brute Force App";
            Width = 750;
            Height = 500;

            btnCreatePassword = new Button();
            btnCreatePassword.Text = "Create password";
            btnCreatePassword.Left = 30;
            btnCreatePassword.Top = 30;
            btnCreatePassword.Width = 160;
            btnCreatePassword.Click += BtnCreatePassword_Click;
            Controls.Add(btnCreatePassword);

            btnStartSingle = new Button();
            btnStartSingle.Text = "Start single-thread";
            btnStartSingle.Left = 30;
            btnStartSingle.Top = 80;
            btnStartSingle.Width = 160;
            btnStartSingle.Click += BtnStartSingle_Click;
            Controls.Add(btnStartSingle);

            btnStartMulti = new Button();
            btnStartMulti.Text = "Start multi-thread";
            btnStartMulti.Left = 210;
            btnStartMulti.Top = 80;
            btnStartMulti.Width = 160;
            btnStartMulti.Click += BtnStartMulti_Click;
            Controls.Add(btnStartMulti);

            btnStop = new Button();
            btnStop.Text = "Stop";
            btnStop.Left = 390;
            btnStop.Top = 80;
            btnStop.Width = 100;
            btnStop.Click += BtnStop_Click;
            Controls.Add(btnStop);

            lblPassword = new Label();
            lblPassword.Left = 30;
            lblPassword.Top = 140;
            lblPassword.Width = 680;
            lblPassword.Text = "Password:";
            Controls.Add(lblPassword);

            lblHash = new Label();
            lblHash.Left = 30;
            lblHash.Top = 170;
            lblHash.Width = 680;
            lblHash.Text = "Hash:";
            Controls.Add(lblHash);

            progressBar = new ProgressBar();
            progressBar.Left = 30;
            progressBar.Top = 220;
            progressBar.Width = 650;
            progressBar.Height = 25;
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            Controls.Add(progressBar);

            lblResult = new Label();
            lblResult.Left = 30;
            lblResult.Top = 270;
            lblResult.Width = 680;
            lblResult.Text = "Found password:";
            Controls.Add(lblResult);

            lblTime = new Label();
            lblTime.Left = 30;
            lblTime.Top = 305;
            lblTime.Width = 680;
            lblTime.Text = "Elapsed time:";
            Controls.Add(lblTime);

            lblThreads = new Label();
            lblThreads.Left = 30;
            lblThreads.Top = 340;
            lblThreads.Width = 680;
            lblThreads.Text = "Threads:";
            Controls.Add(lblThreads);

            lblCompare = new Label();
            lblCompare.Left = 30;
            lblCompare.Top = 375;
            lblCompare.Width = 680;
            lblCompare.Text = "Performance:";
            Controls.Add(lblCompare);
        }

        private void BtnCreatePassword_Click(object sender, EventArgs e)
        {
            PasswordCreator creator = new PasswordCreator();
            HashHelper hashHelper = new HashHelper();

            createdPassword = creator.CreatePassword();
            targetHash = hashHelper.HashPassword(createdPassword);

            lblPassword.Text = "Password: " + createdPassword;
            lblHash.Text = "Hash: " + targetHash;

            lblResult.Text = "Found password:";
            lblTime.Text = "Elapsed time:";
            lblThreads.Text = "Threads:";
            lblCompare.Text = "Performance:";
            progressBar.Value = 0;
        }

        private async void BtnStartSingle_Click(object sender, EventArgs e)
        {
            if (targetHash == "")
            {
                MessageBox.Show("First create password.");
                return;
            }

            cts = new CancellationTokenSource();
            progressBar.Value = 20;

            BruteForceSingle brute = new BruteForceSingle();

            CrackResult result = await Task.Run(() =>
            {
                return brute.Start(targetHash, cts.Token);
            });

            singleTime = result.Seconds;
            ShowResult(result, "Single-thread");
        }

        private async void BtnStartMulti_Click(object sender, EventArgs e)
        {
            if (targetHash == "")
            {
                MessageBox.Show("First create password.");
                return;
            }

            cts = new CancellationTokenSource();
            progressBar.Value = 20;

            BruteForceMulti brute = new BruteForceMulti();

            CrackResult result = await Task.Run(() =>
            {
                return brute.Start(targetHash, cts);
            });

            multiTime = result.Seconds;
            ShowResult(result, "Multi-thread");
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
                lblResult.Text = "Stopped by user";
            }
        }

        private void ShowResult(CrackResult result, string mode)
        {
            progressBar.Value = 100;

            if (result.Stopped && string.IsNullOrEmpty(result.FoundPassword))
            {
                lblResult.Text = mode + " stopped";
            }
            else
            {
                lblResult.Text = mode + " found password: " + result.FoundPassword;
            }

            lblTime.Text = "Elapsed time: " + result.Seconds.ToString("0.000") + " s";
            lblThreads.Text = "Threads used: " + result.ThreadsUsed +
                              " | Checked: " + result.CheckedCount;

            if (singleTime > 0 && multiTime > 0)
            {
                lblCompare.Text = "Performance: Single-thread " +
                                  singleTime.ToString("0.000") +
                                  " s, Multi-thread " +
                                  multiTime.ToString("0.000") + " s";
            }
        }
    }
}
