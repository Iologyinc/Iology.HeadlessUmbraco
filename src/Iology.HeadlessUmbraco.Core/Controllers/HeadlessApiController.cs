/**
 * Copyright (c) 2023 Vertica
 * Copyright (c) 2023 I-ology
 */

using Iology.HeadlessUmbraco.Core.Extensions;
using Iology.HeadlessUmbraco.Core.Models;
using Iology.HeadlessUmbraco.Core.Rendering;
using Iology.HeadlessUmbraco.Core.Rendering.Output;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;

namespace Iology.HeadlessUmbraco.Core.Controllers;

public abstract class HeadlessApiController : UmbracoApiController
{
    protected virtual IContentElementBuilder ContentElementBuilder { get; }
    protected virtual IOutputRenderer OutputRenderer { get; }
    protected virtual IPageDataBuilder PageDataBuilder { get; }
    protected virtual UmbracoHelper UmbracoHelper { get; }

    protected HeadlessApiController(
        IContentElementBuilder contentElementBuilder,
        IOutputRenderer outputRenderer,
        IPageDataBuilder pageDataBuilder,
        UmbracoHelper umbracoHelper)
    {
        ContentElementBuilder = contentElementBuilder;
        OutputRenderer = outputRenderer;
        PageDataBuilder = pageDataBuilder;
        UmbracoHelper = umbracoHelper;
    }

    protected virtual async Task<IActionResult> ContentForAsync(int id, CancellationToken cancellationToken)
        => await ContentResultForAsync(UmbracoHelper.Content(id), cancellationToken).ConfigureAwait(false);

    protected virtual async Task<IActionResult> ContentForAsync(Guid id, CancellationToken cancellationToken)
        => await ContentResultForAsync(UmbracoHelper.Content(id), cancellationToken).ConfigureAwait(false);

    protected virtual async Task<IActionResult> ContentResultForAsync(IPublishedContent? content, CancellationToken cancellationToken)
        => content != null
            ? OutputRenderer.ActionResult(await ContentElementForAsync(content, cancellationToken).ConfigureAwait(false))
            : NotFound();

    protected virtual async Task<IContentElement> ContentElementForAsync(IPublishedContent content, CancellationToken cancellationToken)
        => await ContentElementBuilder.ContentElementForAsync(content, cancellationToken).ConfigureAwait(false);

    protected virtual async Task<IActionResult> PageForAsync(int id, CancellationToken cancellationToken)
        => await PageResultForAsync(UmbracoHelper.Content(id), cancellationToken).ConfigureAwait(false);

    protected virtual async Task<IActionResult> PageForAsync(Guid id, CancellationToken cancellationToken)
        => await PageResultForAsync(UmbracoHelper.Content(id), cancellationToken).ConfigureAwait(false);

    protected virtual async Task<IActionResult> PageResultForAsync(IPublishedContent? content, CancellationToken cancellationToken)
        => content != null
            ? OutputRenderer.ActionResult(await PageDataForAsync(content, cancellationToken).ConfigureAwait(false))
            : NotFound();

    protected virtual async Task<IPageData> PageDataForAsync(IPublishedContent content, CancellationToken cancellationToken)
        => await PageDataBuilder.BuildPageDataAsync(content, cancellationToken).ConfigureAwait(false);
}
