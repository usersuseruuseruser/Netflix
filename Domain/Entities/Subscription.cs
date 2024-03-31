﻿namespace Domain.Entities;

public class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxResolution { get; set; }

    public List<ContentBase> AccessibleContent { get; set; } = null!;
}