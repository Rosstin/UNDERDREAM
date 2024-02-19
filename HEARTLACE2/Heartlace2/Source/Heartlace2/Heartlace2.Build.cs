// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class Heartlace2 : ModuleRules
{
	public Heartlace2(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore", "EnhancedInput" });
	}
}
