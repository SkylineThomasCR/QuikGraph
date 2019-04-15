﻿using System;
using System.Diagnostics;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// The default implementation of an <see cref="IUndirectedEdge{TVertex}"/> that supports tagging.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TTag">Tag type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [DebuggerDisplay("{" + nameof(Source) + "}<->{" + nameof(Target) + "}:{" + nameof(Tag) + "}")]
    public class TaggedUndirectedEdge<TVertex, TTag> : UndirectedEdge<TVertex>, ITagged<TTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedUndirectedEdge{TVertex, TTag}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="tag">Edge tag.</param>
        public TaggedUndirectedEdge([NotNull] TVertex source, [NotNull] TVertex target, [CanBeNull] TTag tag)
            : base(source, target)
        {
#if SUPPORTS_CONTRACTS
            Contract.Ensures(Equals(Tag, tag));
#endif

            _tag = tag;
        }

        /// <inheritdoc />
        public event EventHandler TagChanged;

        /// <summary>
        /// Event invoker for <see cref="TagChanged"/> event.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        protected virtual void OnTagChanged([NotNull] EventArgs args)
        {
            TagChanged?.Invoke(this, args);
        }

        private TTag _tag;

        /// <inheritdoc />
        public TTag Tag
        {
            get => _tag;
            set
            {
                if (Equals(_tag, value))
                    return;

                _tag = value;
                OnTagChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.TaggedUndirectedEdgeFormatString, Source, Target, Tag);
        }
    }
}
