// ***********************************************************************
// Assembly         :
// Author           : Daniel McCluskey - s1600056
// Created          : 05-22-2018
//
// Last Modified By : Daniel McCluskey - s1600056
// Last Modified On : 05-22-2018
// ***********************************************************************
#ifndef _AUDIO_COMPONENT_H_
#define _AUDIO_COMPONENT_H_

//includes
#include "Component.h"
#include <fmod.hpp>
#include <fmod_studio.hpp>
#include <vector>

enum AudioType
{
	AUDIO_AMBIENT,
	AUDIO_ATTACK,
	AUDIO_DEATH
};

class Entity;

class AudioComponent : public Component
{
public:
	/// <summary>
	/// Initializes a new instance of the <see cref="AudioComponent"/> class.
	/// </summary>
	/// <param name="pOwner">The Owner entity class member.</param>
	AudioComponent(Entity* pOwner);

	/// <summary>
	/// Finalizes an instance of the <see cref="AudioComponent"/> class.
	/// </summary>
	~AudioComponent();

	/// <summary>
	/// Updates the class member.
	/// </summary>
	/// <param name="a_fDeltaTime">The Time since the last frame.</param>
	virtual void Update(float a_fDeltaTime);
	/// <summary>
	/// Starts the specified sound.
	/// </summary>
	/// <param name="a_iSoundType">Type of sound to start.</param>
	void StartSound(int a_iSoundType);
	/// <summary>
	/// Adds the sound to the event holder vector.
	/// </summary>
	/// <param name="a_pEventName">Name of the event.</param>
	/// <param name="a_bStartInstantly">Start the sound instantly.</param>
	/// <param name="a_iSoundType">Type of sound.</param>
	void AddSound(FMOD::Studio::EventDescription*& a_pEventName, bool a_bStartInstantly, int a_iSoundType);
	/// <summary>
	/// Stops the sound.
	/// </summary>
	/// <param name="a_iSoundType">Type of sound to stop.</param>
	void StopSound(int a_iSoundType);
	/// <summary>
	/// Updates the event position.
	/// </summary>
	/// <param name="a_v3Pos">Position to play the sound at.</param>
	void UpdateEventPosition(glm::vec3 a_v3Pos);

	/// <summary>
	/// FMOD Position Attributes
	/// </summary>
	FMOD_3D_ATTRIBUTES Attributes3D = { { 0 } };
	/// <summary>
	/// The event instance holder
	/// </summary>
	std::vector<FMOD::Studio::EventInstance*> EventHolder;
	/// <summary>
	/// The event type
	/// </summary>
	std::vector<int> EventType;
};

#endif // !_AUDIO_COMPONENT_H_
