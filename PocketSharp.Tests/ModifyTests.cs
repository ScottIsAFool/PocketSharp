﻿using PocketSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PocketSharp.Tests
{
  public class ModifyTests : TestsBase
  {
    public ModifyTests() : base() { }


    [Fact]
    public async Task IsAnItemArchivedAndUnarchived()
    {
      PocketItem item = await Setup();

      Assert.True(await client.Archive(item));

      item = await GetItemById(item.ID, true);

      Assert.True(item.IsArchive);

      Assert.True(await client.Unarchive(item));

      item = await GetItemById(item.ID);

      Assert.False(item.IsArchive);
    }


    [Fact]
    public async Task IsAnItemFavoritedAndUnfavorited()
    {
      PocketItem item = await Setup();

      Assert.True(await client.Favorite(item));

      item = await GetItemById(item.ID);

      Assert.True(item.IsFavorite);

      Assert.True(await client.Unfavorite(item));

      item = await GetItemById(item.ID);

      Assert.False(item.IsFavorite);
    }


    [Fact]
    public async Task IsAnItemDeleted()
    {
      PocketItem item = await Setup();

      Assert.True(await client.Delete(item));

      item = await GetItemById(item.ID);

      Assert.Null(item);
    }


    [Fact]
    public async Task AreMultipleActionsSent()
    {
      PocketItem item = await Setup();

      bool success = await client.SendActions(new List<PocketAction>()
      {
        new PocketAction() { Action = "favorite", ID = item.ID },
        new PocketAction() { Action = "tags_add", ID = item.ID, Tags = new string[] { "new_tag", "another_tag" } },
        new PocketAction() { Action = "archive", ID = item.ID },
        new PocketAction() { Action = "tag_rename", ID = item.ID, OldTag = "social", NewTag = "not_social" }
      });

      Assert.True(success);

      item = await client.Get(item.ID);

      Assert.True(item.IsFavorite);
      Assert.True(item.IsArchive);
      Assert.NotNull(item.Tags.Single<PocketTag>(tag => tag.Name == "new_tag"));
      Assert.NotNull(item.Tags.Single<PocketTag>(tag => tag.Name == "another_tag"));
      Assert.NotNull(item.Tags.Single<PocketTag>(tag => tag.Name == "not_social"));
    }


    private async Task<PocketItem> Setup()
    {
      PocketItem item = await client.Add(
        uri: new Uri("https://github.com"),
        tags: new string[] { "github", "code", "social" }
      );

      itemsToDelete.Add(item.ID);

      return await GetItemById(item.ID);
    }


    private async Task<PocketItem> GetItemById(int id, bool archive = false)
    {
      List<PocketItem> items = await client.Get(state: archive ? State.archive : State.unread);
      PocketItem itemDesired = null;

      items.ForEach(itm =>
      {
        if (itm.ID == id)
        {
          itemDesired = itm;
        }
      });

      return itemDesired;
    }
  }
}
