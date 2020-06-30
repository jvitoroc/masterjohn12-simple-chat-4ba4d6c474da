using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace ClientConsole
{
    public abstract class CustomWindow
    {
        public Action<CustomWindow> OnClose;
        public Window Win { get => win; }
        public bool Shown { get => shown;}

        protected Window win;

        private bool shown;
        private static CustomWindow CurrentShowWindow = null;
        private static Toplevel Top = new Toplevel() {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        public virtual void Show()
        {
            if (!shown)
            {
                if (!Application.Top.Subviews.Contains(Top))
                {
                    Application.Top.Add(Top);
                }

                if (CurrentShowWindow != null)
                    CurrentShowWindow.Close();

                Top.Add(win);
                CurrentShowWindow = this;
                shown = true;
            }
        }

        public virtual void Close()
        {
            if (shown)
            {
                if (OnClose != null)
                    OnClose(this);

                Top.Remove(win);
                CurrentShowWindow = null;
                shown = false;
            }
        }
    }
}
