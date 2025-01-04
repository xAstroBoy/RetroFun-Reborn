﻿using System;
using System.IO;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Xabbo.Core.Serialization;

namespace Xabbo.Core.GameData;

/// <summary>
/// Defines a map of product code to <see cref="ProductInfo"/>.
/// </summary>
public sealed class ProductData : IReadOnlyDictionary<string, ProductInfo>
{
    public static ProductData Load(string filepath) => new(Json.ProductData.ProductDataLoader.Load(filepath));


    private readonly ImmutableDictionary<string, ProductInfo> _map;

    public ProductInfo this[string key] => _map[key];
    public IEnumerable<string> Keys => _map.Keys;
    public IEnumerable<ProductInfo> Values => _map.Values;
    public int Count => _map.Count;

    internal ProductData(Xabbo.Core.GameData.Json.ProductData.ProductData productDataProxy)
    {
        Dictionary<string, ProductInfo> map = [];

        foreach (var productInfoProxy in productDataProxy.ProductDataContainer.Products)
        {
            ProductInfo productInfo = new(productInfoProxy);

            if (!map.TryAdd(productInfo.Code, productInfo))
            {
                System.Diagnostics.Debug.WriteLine($"Duplicate product code '{productInfo.Code}' in product data.");
            }
        }

        _map = map.ToImmutableDictionary();
    }

    public bool ContainsKey(string key) => _map.ContainsKey(key);
    public IEnumerator<KeyValuePair<string, ProductInfo>> GetEnumerator() => _map.GetEnumerator();
    public bool TryGetValue(string key, [NotNullWhen(true)] out ProductInfo? value) => _map.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Defines a product information.
/// </summary>
/// <param name="Code">The product code.</param>
/// <param name="Name">The product name.</param>
/// <param name="Description">The product description.</param>
public sealed record ProductInfo(string Code, string Name, string Description)
{
    internal ProductInfo(Xabbo.Core.GameData.Json.ProductData.ProductInfo proxy)
        : this(proxy.Code, proxy.Name, proxy.Description)
    { }
}
