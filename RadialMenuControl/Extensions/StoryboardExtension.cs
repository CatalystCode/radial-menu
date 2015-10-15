using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace RadialMenuControl.Extensions
{
    public static class StoryboardExtension
    {
        /// <summary>
        /// Plays the storyboard and returns an awaitable Task
        /// </summary>
        /// <param name="storyboard"></param>
        /// <returns></returns>
        public static Task PlayAsync(this Storyboard storyboard)
        {
            var tcs = new TaskCompletionSource<bool>();
            if (storyboard == null)
                tcs.SetException(new ArgumentNullException());
            else
            {
                EventHandler<object> onComplete = null;
                onComplete = (s, e) => {
                    storyboard.Completed -= onComplete;
                    tcs.SetResult(true);
                };
                storyboard.Completed += onComplete;
                storyboard.Begin();
            }
            return tcs.Task;
        }
    }
}
