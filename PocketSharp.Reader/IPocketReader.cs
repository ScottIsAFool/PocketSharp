﻿using PocketSharp.Models;
using System;
using System.Threading.Tasks;

namespace PocketSharp
{
  public interface IPocketReader
  {
    /// <summary>
    /// Reads article content from the given URI.
    /// This method does not use the official Article View API, which is private.
    /// The PocketReader is based on a custom PCL port of NReadability and SgmlReader.
    /// </summary>
    /// <param name="uri">An URI.</param>
    /// <param name="bodyOnly">if set to <c>true</c> [only body is returned].</param>
    /// <param name="noHeadline">if set to <c>true</c> [no headline (h1) is included].</param>
    /// <returns>A Pocket article with extracted content and title.</returns>
    /// <exception cref="PocketRequestException"></exception>
    Task<PocketArticle> Read(Uri uri, bool bodyOnly = true, bool noHeadline = false);

    /// <summary>
    /// Reads article content from the given PocketItem.
    /// This method does not use the official Article View API, which is private.
    /// The PocketReader is based on a custom PCL port of NReadability and SgmlReader.
    /// </summary>
    /// <param name="item">The pocket item.</param>
    /// <param name="bodyOnly">if set to <c>true</c> [only body is returned].</param>
    /// <param name="noHeadline">if set to <c>true</c> [no headline (h1) is included].</param>
    /// <returns>
    /// A Pocket article with extracted content and title.
    /// </returns>
    /// <exception cref="PocketRequestException"></exception>
    Task<PocketArticle> Read(PocketItem item, bool bodyOnly = true, bool noHeadline = false);
  }
}