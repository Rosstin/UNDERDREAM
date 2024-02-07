// Copyright Epic Games, Inc. All Rights Reserved.

#include "HeartlaceGameMode.h"
#include "HeartlaceCharacter.h"
#include "UObject/ConstructorHelpers.h"

AHeartlaceGameMode::AHeartlaceGameMode()
	: Super()
{

	UE_LOG(LogTemp, Warning, TEXT("AHeartlaceGameMode"));

	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;


}
