using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace VegasProData.General
{
    public static class MessageBoxes
    {
        static DialogResult Base(
            string message,
            string title = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None
        )
        {
            return MessageBox.Show(
                message,
                title,
                buttons,
                icon
            );
        }

        public static DialogResult Warning(
            string message,
            string title = "Warning",
            [CallerMemberName] string callerName = ""
            )
        {
            return Base(
#if DEBUG
                callerName + "\n" +
#endif
                message,
                title,
                icon: MessageBoxIcon.Warning
            );
        }

        public static DialogResult Error(
            string message,
            string title = "Something went wrong",
            [CallerMemberName] string callerName = ""
            )
        {
            return Base(
#if DEBUG
                callerName + "\n" +
#endif
                message,
                title,
                icon: MessageBoxIcon.Error
            );
        }

        public static DialogResult Error(
            Exception ex,
            string title = "Something went wrong",
            [CallerMemberName] string callerName = ""
        )
        {
            return Base(
#if DEBUG
                callerName + "\n" + ex.StackTrace + "\n" +
#endif
                ex.Message,
                title,
                icon: MessageBoxIcon.Error
            );
        }

        public static DialogResult OK(string message, string title)
        {
            return Base(
                message,
                title
            );
        }

        public static DialogResult YesNo(string message, string title)
        {
            return MessageBox.Show(
                message,
                title,
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question
            );
        }
    }
}
