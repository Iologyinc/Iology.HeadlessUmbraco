﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Vertica.Umbraco.Headless.Core.Extensions;
using Vertica.Umbraco.Headless.Core.Models;

namespace Vertica.Umbraco.Headless.Core.Rendering.PropertyRenderers
{
	public class BlockListPropertyRenderer : IPropertyRenderer
	{
		public string PropertyEditorAlias => Constants.PropertyEditors.Aliases.BlockList;

		public Type TypeFor(IPublishedPropertyType propertyType) => typeof(ContentElementWithSettings[]);

        public virtual async Task<object> ValueForAsync(object umbracoValue,
            IPublishedProperty property,
            IContentElementBuilder contentElementBuilder,
            CancellationToken cancellationToken) => umbracoValue is IEnumerable<BlockListItem> items
                ? await items.ToArrayAsync(async i => await contentElementBuilder.ContentElementWithSettingsForAsync(i.Content, i.Settings, cancellationToken).ConfigureAwait(false)).ConfigureAwait(false)
                : null;
    }
}
