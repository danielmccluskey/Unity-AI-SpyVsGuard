// ***********************************************************************
// Assembly         :
// Author           : Daniel McCluskey - s1600056
// Created          : 05-22-2018
//
// Last Modified By : Daniel McCluskey - s1600056
// Last Modified On : 05-22-2018
// ***********************************************************************

#ifndef _ENTITY_H
#define _ENTITY_H

// Includes
#include <map>
#include <vector>

class BasicNetworkPlayer;
class Component;
enum COMPONENT_TYPE;
enum PEER_TYPE;

class Entity
{
public:
	/// <summary>
	/// Initializes a new instance of the <see cref="Entity"/> class.
	/// </summary>
	Entity();
	/// <summary>
	/// Finalizes an instance of the <see cref="Entity"/> class.
	/// </summary>
	~Entity();

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
	virtual void Draw(unsigned int a_uiProgramID, unsigned int a_uiVBO, unsigned int a_uiIBO);

	/// <summary>
	/// Adds the component.
	/// </summary>
	/// <param name="a_pComponent">Component type</param>
	void AddComponent(Component* a_pComponent)
	{
		m_apComponentList.push_back(a_pComponent);
	}

	/// <summary>
	/// Finds the type of the component.
	/// </summary>
	/// <param name="eComponentType">Type of component.</param>
	/// <returns>Component *.</returns>
	Component* FindComponentOfType(COMPONENT_TYPE eComponentType);

	/// <summary>
	/// Gets the entity identifier.
	/// </summary>
	/// <returns>unsigned int.</returns>
	unsigned int GetEntityID() { return m_uiEntityID; }

	/// <summary>
	/// Gets the entity list.
	/// </summary>
	/// <returns>const std.map&lt;const unsigned int,Entity*&gt;.</returns>
	const static std::map<const unsigned int, Entity*> GetEntityList() { return s_xEntityList; }

private:
	/// <summary>
	/// The entity identifier
	/// </summary>
	unsigned int m_uiEntityID;
	/// <summary>
	/// The  component list
	/// </summary>
	std::vector<Component*>m_apComponentList;

	static unsigned int s_uiEntityIDCount;
	static std::map<const unsigned int, Entity*>s_xEntityList;
};

#endif // _ENTITY_H