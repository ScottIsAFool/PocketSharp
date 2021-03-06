﻿using PocketSharp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PocketSharp
{
  public interface IPocketClient
  {
    #region properties
    /// <summary>
    /// callback URLi for API calls
    /// </summary>
    /// <value>
    /// The callback URI.
    /// </value>
    string CallbackUri { get; set; }

    /// <summary>
    /// Accessor for the Pocket API key
    /// see: http://getpocket.com/developer
    /// </summary>
    /// <value>
    /// The consumer key.
    /// </value>
    string ConsumerKey { get; set; }

    /// <summary>
    /// Code retrieved on authentification
    /// </summary>
    /// <value>
    /// The request code.
    /// </value>
    string RequestCode { get; set; }

    /// <summary>
    /// Code retrieved on authentification-success
    /// </summary>
    /// <value>
    /// The access code.
    /// </value>
    string AccessCode { get; set; }
    #endregion

    #region account methods
    /// <summary>
    /// Retrieves the requestCode from Pocket, which is used to generate the Authentication URI to authenticate the user
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException">Authentication methods need a callbackUri on initialization of the PocketClient class</exception>
    /// <exception cref="PocketException"></exception>
    Task<string> GetRequestCode(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Generate Authentication URI from requestCode
    /// </summary>
    /// <param name="requestCode">The requestCode. If no requestCode is supplied, the property from the PocketClient intialization is used.</param>
    /// <returns>
    /// A valid URI to redirect the user to.
    /// </returns>
    /// <exception cref="System.NullReferenceException">Call GetRequestCode() first to receive a request_code</exception>
    Uri GenerateAuthenticationUri(string requestCode = null);

    /// <summary>
    /// Requests the access code and username after authentication
    /// The access code has to permanently be stored within the users session, and should be passed in the constructor for all future PocketClient initializations.
    /// </summary>
    /// <param name="requestCode">The request code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The authenticated user
    /// </returns>
    /// <exception cref="System.NullReferenceException">Call GetRequestCode() first to receive a request_code</exception>
    Task<PocketUser> GetUser(string requestCode = null, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Registers a new account.
    /// Account has to be activated via a activation email sent by Pocket.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="email">The email.</param>
    /// <param name="password">The password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">All parameters are required</exception>
    /// <exception cref="System.FormatException">Invalid email address.
    /// or
    /// Invalid username. Please only use letters, numbers, and/or dashes and between 1-20 characters.
    /// or
    /// Invalid password.</exception>
    /// <exception cref="PocketException"></exception>
    Task<bool> RegisterAccount(string username, string email, string password, CancellationToken cancellationToken = default(CancellationToken));
    #endregion

    #region add methods
    /// <summary>
    /// Adds a new item to pocket
    /// </summary>
    /// <param name="uri">The URL of the item you want to save</param>
    /// <param name="tags">A comma-separated list of tags to apply to the item</param>
    /// <param name="title">This can be included for cases where an item does not have a title, which is typical for image or PDF URLs. If Pocket detects a title from the content of the page, this parameter will be ignored.</param>
    /// <param name="tweetID">If you are adding Pocket support to a Twitter client, please send along a reference to the tweet status id. This allows Pocket to show the original tweet alongside the article.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A simple representation of the saved item which doesn't contain all data (is only returned by calling the Retrieve method)
    /// </returns>
    /// <exception cref="System.FormatException">(1) Uri should be absolute.</exception>
    /// <exception cref="PocketException"></exception>
    Task<PocketItem> Add(Uri uri, string[] tags = null, string title = null, string tweetID = null, CancellationToken cancellationToken = default(CancellationToken));
    #endregion

    #region get methods
    /// <summary>
    /// Retrieves items from pocket
    /// with the given filters
    /// </summary>
    /// <param name="state">The state.</param>
    /// <param name="favorite">The favorite.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="sort">The sort.</param>
    /// <param name="search">The search.</param>
    /// <param name="domain">The domain.</param>
    /// <param name="since">The since.</param>
    /// <param name="count">The count.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<List<PocketItem>> Get(
      State? state = null,
      bool? favorite = null,
      string tag = null,
      ContentType? contentType = null,
      Sort? sort = null,
      string search = null,
      string domain = null,
      DateTime? since = null,
      int? count = null,
      int? offset = null,
      CancellationToken cancellationToken = default(CancellationToken)
    );

    /// <summary>
    /// Retrieves an item by a given ID
    /// Note: The Pocket API contains no method, which allows to retrieve a single item, so all items are retrieved and filtered locally by the ID.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<PocketItem> Get(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Retrieves all items by a given filter
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<List<PocketItem>> Get(RetrieveFilter filter, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Retrieves all available tags.
    /// Note: The Pocket API contains no method, which allows to retrieve all tags, so all items are retrieved and the associated tags extracted.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<List<PocketTag>> GetTags(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Retrieves items by tag
    /// </summary>
    /// <param name="tag">The tag.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<List<PocketItem>> SearchByTag(string tag, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Retrieves items which match the specified search string in title and URI
    /// </summary>
    /// <param name="searchString">The search string.</param>
    /// <param name="searchInUri">if set to <c>true</c> [search in URI].</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Search string length has to be a minimum of 2 chars</exception>
    /// <exception cref="PocketException"></exception>
    Task<List<PocketItem>> Search(string searchString, bool searchInUri = true, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Finds the specified search string in title and URI for an available list of items
    /// </summary>
    /// <param name="availableItems">The available items.</param>
    /// <param name="searchString">The search string.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Search string length has to be a minimum of 2 chars</exception>
    /// <exception cref="PocketException"></exception>
    List<PocketItem> Search(List<PocketItem> availableItems, string searchString);
    #endregion

    #region modify methods
    /// <summary>
    /// Sends multiple actions in one request.
    /// See: http://getpocket.com/developer/docs/v3/modify
    /// </summary>
    /// <param name="actions">The actions.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> SendActions(List<PocketAction> actions, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Archives the specified item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Archive(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Archives the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Archive(PocketItem item, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Un-archives the specified item (alias for Readd).
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Unarchive(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Unarchives the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Unarchive(PocketItem item, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Favorites the specified item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Favorite(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Favorites the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Favorite(PocketItem item, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Un-favorites the specified item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Unfavorite(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Un-favorites the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Unfavorite(PocketItem item, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Deletes the specified item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> Delete(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Deletes the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<bool> Delete(PocketItem item, CancellationToken cancellationToken = default(CancellationToken));
    #endregion

    #region modify tags methods
    /// <summary>
    /// Adds the specified tags to an item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> AddTags(int itemID, string[] tags, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Adds the specified tags to an item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> AddTags(PocketItem item, string[] tags, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes the specified tags from an item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RemoveTags(int itemID, string[] tags, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes the specified tags from an item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="tags">The tag.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RemoveTags(PocketItem item, string[] tags, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes a tag from an item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RemoveTag(int itemID, string tag, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes a tag from an item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RemoveTag(PocketItem item, string tag, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Clears all tags from an item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RemoveTags(int itemID, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Clears all tags from an item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RemoveTags(PocketItem item, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Replaces all existing tags with the given tags in an item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> ReplaceTags(int itemID, string[] tags, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Replaces all existing tags with the given new ones in an item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="tags">The tags.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> ReplaceTags(PocketItem item, string[] tags, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Renames a tag in an item.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="oldTag">The old tag.</param>
    /// <param name="newTag">The new tag name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RenameTag(int itemID, string oldTag, string newTag, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Renames a tag in an item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="oldTag">The old tag.</param>
    /// <param name="newTag">The new tag name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<bool> RenameTag(PocketItem item, string oldTag, string newTag, CancellationToken cancellationToken = default(CancellationToken));
    #endregion

    #region statistics methods
    /// <summary>
    /// Statistics from the user account.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<PocketStatistics> GetUserStatistics(CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Returns API usage statistics.
    /// If a request was made before, the data is returned synchronously from the cache.
    /// Note: This method only works for authenticated users with a given AccessCode.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="PocketException"></exception>
    Task<PocketLimits> GetUsageLimits(CancellationToken cancellationToken = default(CancellationToken));
    #endregion
  }
}