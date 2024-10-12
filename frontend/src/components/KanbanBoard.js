import React, { useState, useEffect } from "react";
import api from "../axiosConfig";
import { useParams } from "react-router-dom";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import './Kanban.css';

const KanbanBoard = () => {
    const { bid } = useParams();
    const [lanes, setLanes] = useState([]);
    const [newLaneTitle, setNewLaneTitle] = useState('');
    const [selectedCard, setSelectedCard] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isUpdating, setIsUpdating] = useState(false);
    useEffect(() => {
        fetchLanes();
    }, [bid]);

    const fetchLanes = async () => {
        try {
            const response = await api.get(`/boards/${bid}/lanes`);
            const lanesData = response.data;
            console.log(lanesData);
            for (let lane of lanesData) {
                // console.log(lane)
                const cardsResponse = await api.get(`/lanes/${lane.lid}/cards`);
                lane.cards = cardsResponse.data;
            }
            setLanes(lanesData);
        } catch (error) {
            console.error("Error fetching lanes and cards:", error);
        }
    };

    const handleCreateLane = () => {
        if (newLaneTitle) {
            api.post(`/boards/${bid}/lanes`, { title: newLaneTitle })
                .then(res => {
                    setLanes([...lanes, {...res.data, cards: []}]);
                    setNewLaneTitle('');
                })
                .catch(err => console.error(err));
        }
    };

    const handleDeleteLane = (lid) => {
        api.delete(`/boards/${bid}/lanes/${lid}`)
            .then(() => setLanes(lanes.filter(lane => lane.lid !== lid)))
            .catch(err => console.error(err));
    };

    const handleEditLaneTitle = (lid, newTitle) => {
        api.put(`/boards/${bid}/lanes/${lid}`, { title: newTitle })
            .then(res => setLanes(lanes.map(lane => (lane.lid === lid ? {...res.data, cards: lane.cards} : lane))))
            .catch(err => console.error(err));
    };

    const handleCreateCard = (lid) => {
        const newCard = { title: "New Issue", label: "Label", description: "", comments: "" };
        api.post(`/lanes/${lid}/cards`, newCard)
            .then(res => {
                setLanes(lanes.map(lane => {
                    if (lane.lid === lid) {
                        return {...lane, cards: [...lane.cards, res.data]};
                    }
                    return lane;
                }));
            })
            .catch(err => console.error(err));
    };

    const handleCardClick = (card) => {
        setSelectedCard(card);
        setIsModalOpen(true);
    };

    const handleSaveCardDetails = () => {
        api.put(`/lanes/${selectedCard.lid}/cards/${selectedCard.cid}`, selectedCard)
            .then(res => {
                setLanes(lanes.map(lane => {
                    if (lane.lid === selectedCard.lid) {
                        return {
                            ...lane,
                            cards: lane.cards.map(card => card.cid === selectedCard.cid ? res.data : card)
                        };
                    }
                    return lane;
                }));
                setIsModalOpen(false);
            })
            .catch(err => console.error(err));
    };

    const handleDeleteCard = () => {
        api.delete(`/lanes/${selectedCard.lid}/cards/${selectedCard.cid}`)
            .then(() => {
                setLanes(lanes.map(lane => {
                    if (lane.lid === selectedCard.lid) {
                        return {
                            ...lane,
                            cards: lane.cards.filter(card => card.cid !== selectedCard.cid)
                        };
                    }
                    return lane;
                }));
                setIsModalOpen(false);
            })
            .catch(err => console.error(err));
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setSelectedCard(null);
    };

    const handleDragEnd = (result) => {
        if (!result.destination || isUpdating) return;
        const { source, destination } = result;

        if (source.droppableId !== destination.droppableId) {
            setIsUpdating(true);
            const sourceLaneId = parseInt(source.droppableId);
            const destLaneId = parseInt(destination.droppableId);
            const sourceLane = lanes.find(lane => lane.lid === sourceLaneId);
            const card = sourceLane.cards[source.index];

            // Optimistically update the UI
            const newLanes = lanes.map(lane => {
                if (lane.lid === sourceLaneId) {
                    return {...lane, cards: lane.cards.filter((_, index) => index !== source.index)};
                }
                if (lane.lid === destLaneId) {
                    const newCards = [...lane.cards];
                    newCards.splice(destination.index, 0, {...card, lid: destLaneId});
                    return {...lane, cards: newCards};
                }
                return lane;
            });

            setLanes(newLanes);

            // Update backend
            api.put(`/lanes/${destLaneId}/cards/${card.cid}`, { ...card, lid: destLaneId })
                .then(() => {
                    setIsUpdating(false);
                })
                .catch(err => {
                    console.error(err);
                    fetchLanes();
                    setIsUpdating(false);
                });
        }
    };

    return (
        <div>
            <h1>Kanban Board</h1>
            <div>
                <input
                    type="text"
                    placeholder="New Lane Title"
                    value={newLaneTitle}
                    onChange={(e) => setNewLaneTitle(e.target.value)}
                />
                <button onClick={handleCreateLane}>Create Lane</button>
            </div>
            <DragDropContext onDragEnd={handleDragEnd}>
                <div className="kanban">
                    {lanes.map((lane) => (
                        <Droppable key={lane.lid} droppableId={`${lane.lid}`}>
                            {(provided) => (
                                <div
                                    className="lane"
                                    {...provided.droppableProps}
                                    ref={provided.innerRef}
                                >
                                    <input
                                        value={lane.title}
                                        onChange={(e) => handleEditLaneTitle(lane.lid, e.target.value)}
                                    />
                                    <button onClick={() => handleDeleteLane(lane.lid)}>Delete Lane</button>
                                    <div className="cards">
                                        {lane.cards.map((card, index) => (
                                            <Draggable key={card.cid} draggableId={`${card.cid}`} index={index}>
                                                {(provided) => (
                                                    <div
                                                        className="card"
                                                        ref={provided.innerRef}
                                                        {...provided.draggableProps}
                                                        {...provided.dragHandleProps}
                                                        onClick={() => handleCardClick(card)}
                                                    >
                                                        <h3>{card.title}</h3>
                                                        <p>{card.label}</p>
                                                    </div>
                                                )}
                                            </Draggable>
                                        ))}
                                        {provided.placeholder}
                                        <button onClick={() => handleCreateCard(lane.lid)}>+ Add Card</button>
                                    </div>
                                </div>
                            )}
                        </Droppable>
                    ))}
                </div>
            </DragDropContext>

            {isModalOpen && (
                <>
                    <div className="card-details-blur"></div>
                    <div className="card-details">
                        <button className="close-button" onClick={handleCloseModal}>×</button>
                        <h2>Edit Card</h2>
                        <form>
                            <label>Title:</label>
                            <input
                                type="text"
                                value={selectedCard.title}
                                onChange={(e) => setSelectedCard({ ...selectedCard, title: e.target.value })}
                            />
                            <label>Label:</label>
                            <input
                                type="text"
                                value={selectedCard.label}
                                onChange={(e) => setSelectedCard({ ...selectedCard, label: e.target.value })}
                            />
                            <label>Description:</label>
                            <textarea
                                value={selectedCard.description}
                                onChange={(e) => setSelectedCard({ ...selectedCard, description: e.target.value })}
                            ></textarea>
                            <label>Comments:</label>
                            <input
                                type="text"
                                value={selectedCard.comments}
                                onChange={(e) => setSelectedCard({ ...selectedCard, comments: e.target.value })}
                            />
                            <div className="button-group">
                                <button type="button" onClick={handleSaveCardDetails}>Save</button>
                                <button type="button" onClick={handleDeleteCard} className="delete-button">Delete Card</button>
                            </div>
                        </form>
                    </div>
                </>
            )}
        </div>
    );
};

export default KanbanBoard;

// import React, { useState, useEffect } from "react";
// import api from "../axiosConfig";
// import { useParams } from "react-router-dom";
// import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
// import './Kanban.css';

// const KanbanBoard = () => {
//     const { bid } = useParams();
//     const [lanes, setLanes] = useState([]);
//     const [newLaneTitle, setNewLaneTitle] = useState('');
//     const [selectedCard, setSelectedCard] = useState(null);
//     const [isModalOpen, setIsModalOpen] = useState(false);

//     useEffect(() => {
//         const fetchLanes = async () => {
//             try {
//                 const response = await fetch(`http://localhost:5126/api/boards/${bid}/lanes`);
//                 const data = await response.json();
//                 setLanes(data);
//             } catch (error) {
//                 console.error("Error fetching lanes:", error);
//             }
//         };
//         fetchLanes();
//     }, [bid]);

//     const handleCreateLane = () => {
//         if (newLaneTitle) {
//             api.post(`/boards/${bid}/lanes`, { title: newLaneTitle })
//                 .then(res => setLanes([...lanes, res.data]))
//                 .catch(err => console.error(err));
//         }
//     };

//     const handleDeleteLane = (lid) => {
//         api.delete(`/boards/${bid}/lanes/${lid}`)
//             .then(() => setLanes(lanes.filter(lane => lane.lid !== lid)))
//             .catch(err => console.error(err));
//     };

//     const handleEditLaneTitle = (lid, newTitle) => {
//         api.put(`/boards/${bid}/lanes/${lid}`, { title: newTitle })
//             .then(res => setLanes(lanes.map(lane => (lane.lid === lid ? res.data : lane))))
//             .catch(err => console.error(err));
//     };

//     const handleCreateCard = (lid) => {
//         const newCard = { title: "New Issue", label: "Label", description: "", comments: "" };
//         api.post(`/lanes/${lid}/cards`, newCard)
//             .then(res => setLanes(lanes.map(lane => {
//                 if (lane.lid === lid) lane.cards.push(res.data);
//                 return lane;
//             })))
//             .catch(err => console.error(err));
//     };

//     const handleCardClick = (card) => {
//         setSelectedCard(card);
//         setIsModalOpen(true);
//     };

//     const handleSaveCardDetails = () => {
//         // Save the card details to backend
//         api.put(`/cards/${selectedCard.cid}`, selectedCard)
//             .then(res => {
//                 setLanes(lanes.map(lane => {
//                     if (lane.lid === selectedCard.lid) {
//                         lane.cards = lane.cards.map(card => card.cid === selectedCard.cid ? selectedCard : card);
//                     }
//                     return lane;
//                 }));
//                 setIsModalOpen(false);
//             })
//             .catch(err => console.error(err));
//     };

//     const handleDragEnd = (result) => {
//         if (!result.destination) return;
//         const { source, destination } = result;

//         if (source.droppableId !== destination.droppableId) {
//             const sourceLane = lanes.find(lane => lane.lid === parseInt(source.droppableId));
//             const destinationLane = lanes.find(lane => lane.lid === parseInt(destination.droppableId));
//             const card = sourceLane.cards[source.index];

//             sourceLane.cards.splice(source.index, 1);
//             destinationLane.cards.splice(destination.index, 0, card);

//             // Update backend
//             api.put(`/cards/${card.cid}`, { ...card, lid: destinationLane.lid })
//                 .catch(err => console.error(err));

//             setLanes([...lanes]);
//         }
//     };

//     return (
//         <div>
//             <h1>Kanban Board</h1>
//             <div>
//                 <input
//                     type="text"
//                     placeholder="New Lane Title"
//                     value={newLaneTitle}
//                     onChange={(e) => setNewLaneTitle(e.target.value)}
//                 />
//                 <button onClick={handleCreateLane}>Create Lane</button>
//             </div>
//             <DragDropContext onDragEnd={handleDragEnd}>
//                 <div className="kanban">
//                     {lanes.map((lane) => (
//                         <Droppable key={lane.lid} droppableId={`${lane.lid}`}>
//                             {(provided) => (
//                                 <div
//                                     className="lane"
//                                     {...provided.droppableProps}
//                                     ref={provided.innerRef}
//                                 >
//                                     <input
//                                         value={lane.title}
//                                         onChange={(e) => handleEditLaneTitle(lane.lid, e.target.value)}
//                                     />
//                                     <button onClick={() => handleDeleteLane(lane.lid)}>Delete Lane</button>
//                                     <div className="cards">
//                                         {lane.cards.map((card, index) => (
//                                             <Draggable key={card.cid} draggableId={`${card.cid}`} index={index}>
//                                                 {(provided) => (
//                                                     <div
//                                                         className="card"
//                                                         ref={provided.innerRef}
//                                                         {...provided.draggableProps}
//                                                         {...provided.dragHandleProps}
//                                                         onClick={() => handleCardClick(card)}
//                                                     >
//                                                         <h3>{card.title}</h3>
//                                                         <p>{card.label}</p>
//                                                     </div>
//                                                 )}
//                                             </Draggable>
//                                         ))}
//                                         {provided.placeholder}
//                                         <button onClick={() => handleCreateCard(lane.lid)}>+ Add Card</button>
//                                     </div>
//                                 </div>
//                             )}
//                         </Droppable>
//                     ))}
//                 </div>
//             </DragDropContext>

//             {isModalOpen && (
//                 <>
//                     <div className="card-details-blur"></div>
//                     <div className="card-details">
//                         <h2>Edit Card</h2>
//                         <form>
//                             <label>Title:</label>
//                             <input
//                                 type="text"
//                                 value={selectedCard.title}
//                                 onChange={(e) => setSelectedCard({ ...selectedCard, title: e.target.value })}
//                             />
//                             <label>Label:</label>
//                             <input
//                                 type="text"
//                                 value={selectedCard.label}
//                                 onChange={(e) => setSelectedCard({ ...selectedCard, label: e.target.value })}
//                             />
//                             <label>Description:</label>
//                             <textarea
//                                 value={selectedCard.description}
//                                 onChange={(e) => setSelectedCard({ ...selectedCard, description: e.target.value })}
//                             ></textarea>
//                             <label>Comments:</label>
//                             <input
//                                 type="text"
//                                 value={selectedCard.comments}
//                                 onChange={(e) => setSelectedCard({ ...selectedCard, comments: e.target.value })}
//                             />
//                             <button type="button" onClick={handleSaveCardDetails}>Save</button>
//                         </form>
//                     </div>
//                 </>
//             )}
//         </div>
//     );
// };

// export default KanbanBoard;
