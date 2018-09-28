// ***********************************************************************
// Assembly         :
// Author           : Daniel McCluskey - s1600056
// Created          : 05-22-2018
//
// Last Modified By : Daniel McCluskey - s1600056
// Last Modified On : 05-22-2018
// ***********************************************************************

#ifndef _COMPONENT_H_
#define _COMPONENT_H_
#include <glm/glm.hpp>

class Entity;

enum COMPONENT_TYPE
{
	AUDIO,
	BULLET,
	MODEL,
	MOVEMENT,
	NETWORK,
	NETWORKID,
	PREDATOR,
	TRANSFORM
};

enum PEER_TYPE
{
	NETWORK_PEER_HOST,
	NETWORK_PEER_PLAYER
};

class Component
{
public:

	/// <summary>
	/// Initializes a new instance of the <see cref="Component"/> class.
	/// </summary>
	/// <param name="a_pOwnerEntity">The Owner Entity.</param>
	Component(Entity* a_pOwnerEntity);
	/// <summary>
	/// Finalizes an instance of the <see cref="Component"/> class.
	/// </summary>
	~Component();
	/// <summary>
	/// Draws the specified a u program identifier.
	/// </summary>
	/// <param name="a_uProgramID">a u program identifier.</param>
	/// <param name="a_uVBO">a u vbo.</param>
	/// <param name="a_uIBO">a u ibo.</param>
	virtual void Draw(unsigned int a_uProgramID, unsigned int a_uVBO, unsigned int a_uIBO) {};
	/// <summary>
	/// Updates the class member.
	/// </summary>
	/// <param name="a_fDeltaTime">The Time since the last frame.</param>
	virtual void Update(float a_fDeltaTime) {};

	/// <summary>
	/// Gets the type of the component.
	/// </summary>
	/// <returns>COMPONENT_TYPE.</returns>
	COMPONENT_TYPE GetComponentType();
	/// <summary>
	/// Gets the owner entity.
	/// </summary>
	/// <returns>Entity *.</returns>
	Entity* GetOwnerEntity();

	/// <summary>
	/// The m p owner entity
	/// </summary>
	Entity* m_pOwnerEntity;
	/// <summary>
	/// The component type
	/// </summary>
	COMPONENT_TYPE m_eComponentType;

private:
};
#endif