using System.DirectoryServices.ActiveDirectory;
using System.Text.Json;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Main_Program
{
    public partial class Form1 : Form
    {
        const string exercise_json_path = "C:\\Users\\ipepd\\source\\repos\\Main Program\\Main Program\\Resources\\exercises.json";
        public Form1()
        {
            InitializeComponent();

            string[] weight_exercises = { "Bicep Curl", "Tricep Pushdown", "Lat Pulldown", "Chest Press" };
            weight_exercise_names.Items.AddRange(weight_exercises);

            string[] duration_exercises = { "Running", "Walking", "Stair Stepper", "Eliptical" };
            duration_exercises_names.Items.AddRange(duration_exercises);

        }

        //private void pictureBox1_Click(object sender, EventArgs e)
        //{

        //}
        private void log_exercise_button_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void log_exercise_button_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void duration_based_check_CheckedChanged(object sender, EventArgs e)
        {
            if (duration_based_check.Checked)
            {
                weight_based_input.Visible = false;
                duration_based_input.Visible = true;
            }
            else
            {
                weight_based_input.Visible = true;
            }

        }

        private void gym_check_CheckedChanged(object sender, EventArgs e)
        {
            if (gym_check.Checked)
            {
                gym_label.Visible = true;
                gym_text_box.Visible = true;
            }
            else
            {
                gym_label.Visible = false;
                gym_text_box.Visible = false;
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            string exercise_name = weight_exercise_names.SelectedItem.ToString();
            string gym_name = string.Empty;
            var entry = (object)null;

            undo_button.Visible = true;
            undo_button.BringToFront();
            undo_label.Visible = true;
            undo_label.BringToFront();

            if (gym_check.Checked)
            {
                gym_name = gym_text_box.Text.ToString();
            }

            if (duration_based_check.Checked)
            {
                string hours = hours_textbox.Text.ToString();
                string minutes = minutes_textbox.Text.ToString();
                string seconds = seconds_textbox.Text.ToString();
                string duration_date = dateTimePicker2.Text.ToString();

                entry = new { exercise = exercise_name, hours = hours, minutes = minutes, seconds = seconds, date = duration_date, gym_name = gym_name };
            }
            else
            {
                string weight_per_set = weight_input.Text.ToString();
                string weight_date = dateTimePicker1.Text.ToString();
                gym_name = gym_text_box.Text.ToString();

                entry = new { exercise = exercise_name, weight = weight_per_set, date = weight_date, gym_name = gym_name };
            }

            string json = JsonSerializer.Serialize(entry);

            File.AppendAllText(exercise_json_path, json + Environment.NewLine);
        }

        private void drop_down_button_Click(object sender, EventArgs e)
        {
            sidebar.BringToFront();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            sidebar.SendToBack();
        }

        private void log_exercise_button_Click(object sender, EventArgs e)
        {
            logExercise.BringToFront();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            homePage.BringToFront();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            sidebar.BringToFront();
        }

        private void populateExercises(string date_filter)
        {
            string[] lines = File.ReadAllLines(exercise_json_path);
            foreach (string line in lines)
            {
                using var doc = JsonDocument.Parse(line);
                var root = doc.RootElement;
                string current_date = root.GetProperty("date").GetString();

                if (current_date == date_filter)
                {
                    string current_exercise = root.GetProperty("exercise").GetString();
                    remove_exercise_combobox.Items.Add(current_exercise);
                }
            }

            if (remove_exercise_combobox.Items.Count == 0)
            {
                remove_exercise_combobox.Text = ("No exercises found");
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Startup population
            string date_filter = remove_date_picker.Text.ToString();
            populateExercises(date_filter);

            remove_exercise.Visible = true;
            remove_exercise.BringToFront();
        }

        private void remove_date_picker_ValueChanged(object sender, EventArgs e)
        {
            //All subsequent populations
            string date_filter = remove_date_picker.Text.ToString();

            remove_exercise_combobox.Items.Clear();
            populateExercises(date_filter);

        }

        private void delete_Log(string date_filter, string exercise_name)
        {
            string[] lines = File.ReadAllLines(exercise_json_path);

            var filtered_lines = lines.Where(line =>
            !(line.Contains(exercise_name) && line.Contains(date_filter))).ToList();

            File.WriteAllLines(exercise_json_path, filtered_lines.Where(line => !string.IsNullOrWhiteSpace(line)));
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            string date_filter = remove_date_picker.Text.ToString();
            string exercise_name = remove_exercise_combobox.Text.ToString();

            delete_Log(date_filter, exercise_name);
            status_message.Text = "Your " + exercise_name + " entry at " + date_filter + " has succesfully been deleted!";
            status_message.Visible = true;

            if (remove_exercise_combobox.Items.Count == 0)
            {
                remove_exercise_combobox.Text = ("No exercises found");
            }
            else
            {
                remove_exercise_combobox.Text = "";
            }

            remove_exercise_combobox.Items.Remove(exercise_name);

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            remove_exercise.Visible = false;
            remove_exercise.SendToBack();
        }

        private void removeLastLog()
        {
            var lines = File.ReadAllLines(exercise_json_path).ToList();

            lines.RemoveAt(lines.Count - 1);
            File.WriteAllLines(exercise_json_path, lines);
        }
        private void undo_button_Click(object sender, EventArgs e)
        {
            undo_label.Text = "Undid Previous Log!";
            undo_label.Visible = true;
            undo_label.BringToFront();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += (s, e) =>
            {
                undo_button.Visible = false;
                undo_button.SendToBack();

                undo_label.Visible = false;
                undo_label.BringToFront();
                timer.Stop();
            };
            timer.Start();

            removeLastLog();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            logExercise.Visible = true;
            logExercise.BringToFront();

            duration_based_check.Checked = true;
        }
    }
}
