using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jonas_Sage_Importer {
    class UtilityMethods {
        public static void ShowMessageBox(string description, string title) {
            using (var dummy = new Form() { TopMost = true }) {
                MessageBox.Show(dummy, description, title);
            }
        }

        public static void ShowMessageBox(string description) {
            using (var dummy = new Form() { TopMost = true }) {
                MessageBox.Show(dummy, description);
            }
        }

        public static DialogResult ShowMessageBox(string description, string title, MessageBoxButtons buttons, MessageBoxIcon icon) {
            using (var dummy = new Form() { TopMost = true }) {
                DialogResult v = MessageBox.Show(dummy, description, title, buttons, icon);
                return v;
            }
        }
    }
}
