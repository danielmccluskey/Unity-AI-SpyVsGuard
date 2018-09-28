// ***********************************************************************
// Assembly         :
// Author           : Daniel McCluskey - s1600056
// Created          : 05-22-2018
//
// Last Modified By : Daniel McCluskey - s1600056
// Last Modified On : 05-22-2018
// ***********************************************************************

#ifndef _ENEMY_H
#define _ENEMY_H

// Includes
#include "Entity.h"

#include <fmod.hpp>
#include <fmod_studio.hpp>

class Enemy : public Entity
{
public:
	/// <summary>
	/// Initializes a new instance of the <see cref="Enemy"/> class.
	/// </summary>
	/// <param name="a_xModel">a x model.</param>
	/// <param name="a_iEnemyType">Type of a i enemy.</param>
	Enemy(PEER_TYPE a_PEERTYPE);
	/// <summary>
	/// Finalizes an instance of the <see cref="Enemy"/> class.
	/// </summary>
	~Enemy();

	/// <summary>
	/// Updates the class member.
	/// </summary>
	/// <param name="a_fDeltaTime">The Time since the last frame.</param>
	virtual void Update(float a_fDeltaTime);
	/// <summary>
	/// Draws the specified a UI program identifier.
	/// </summary>
	/// <param name="a_uiProgramID">a UI program identifier.</param>
	/// <param name="a_uiVBO">a UI vbo.</param>
	/// <param name="a_uiIBO">a UI ibo.</param>
	void Draw(unsigned int a_uiProgramID, unsigned int a_uiVBO, unsigned int a_uiIBO);

	/// <summary>
	/// Adds the sound to the audio component.
	/// </summary>
	/// <param name="a_bStartInstantly">a b start instantly.</param>
	/// <param name="a_EventName">Name of a event.</param>
	/// <param name="a_iSoundType">Type of a i sound.</param>
	void AddSound(bool a_bStartInstantly, FMOD::Studio::EventDescription*& a_EventName, int a_iSoundType);
};

#endif //_ENEMY_H