using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SourceGit.UI {

    /// <summary>
    ///     Dialog to add new submodule.
    /// </summary>
    public partial class AddSubmodule : UserControl {
        private Git.Repository repo = null;

        /// <summary>
        ///     Submodule's repository URL.
        /// </summary>
        public string RepoURL { get; set; }

        /// <summary>
        ///     Submodule's relative path.
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="opened"></param>
        public AddSubmodule(Git.Repository opened) {
            repo = opened;
            InitializeComponent();
        }

        /// <summary>
        ///     Show this dialog.
        /// </summary>
        /// <param name="repo"></param>
        public static void Show(Git.Repository repo) {
            repo.GetPopupManager()?.Show(new AddSubmodule(repo));
        }

        #region EVENTS
        private async void Sure(object sender, RoutedEventArgs e) {
            txtRepoUrl.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            if (Validation.GetHasError(txtRepoUrl)) return;

            txtPath.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            if (Validation.GetHasError(txtPath)) return;

            var recursive = chkRecursive.IsChecked == true;
            var popup = repo.GetPopupManager();

            popup?.Lock();
            await Task.Run(() => repo.AddSubmodule(RepoURL, LocalPath, recursive, msg => {
                popup?.UpdateStatus(msg);
            }));
            popup?.Close(true);
        }

        private void Cancel(object sender, RoutedEventArgs e) {
            repo.GetPopupManager()?.Close();
        }
        #endregion
    }
}
