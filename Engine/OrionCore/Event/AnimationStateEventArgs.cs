using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Event
{
    /// <summary>
    /// AnimationStateEventHandler delegate.
    /// </summary>
    /// <param name="sender">The object that invoked the event.</param>
    /// <param name="args">The event arguments.</param>
    public delegate void AnimationStateEventHandler(object sender, AnimationStateEventArgs args);

    /// <summary>
    /// Event args class for relaying an AnimationState.
    /// </summary>
    public class AnimationStateEventArgs : EventArgs
    {
        /// <summary>
        /// The current animation state.
        /// </summary>
        public AnimationState State { get; set; }

        public AnimationStateEventArgs(AnimationState state)
        {
            this.State = state;
        }
    }
}
