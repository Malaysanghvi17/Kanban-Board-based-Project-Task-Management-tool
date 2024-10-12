package com.example.kanban_backend.Controller;

import com.example.kanban_backend.Model.CardIssues;
import com.example.kanban_backend.Model.LaneColumns;
import com.example.kanban_backend.Repository.CardIssuesRepository;
import com.example.kanban_backend.Repository.LaneColumnsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/lanes/{laneId}/cards")
public class CardIssuesController {

    @Autowired
    private CardIssuesRepository cardRepository;

    @Autowired
    private LaneColumnsRepository laneRepository;

    @GetMapping
    public List<CardIssues> getCardsByLaneId(@PathVariable int laneId) {
        LaneColumns lane = laneRepository.findById(laneId).orElseThrow(() -> new RuntimeException("Lane not found"));
        return lane.getCards();
    }

    @PostMapping
    public CardIssues createCard(@PathVariable int laneId, @RequestBody CardIssues card) {
        LaneColumns lane = laneRepository.findById(laneId).orElseThrow(() -> new RuntimeException("Lane not found"));
        card.setLane(lane);
        return cardRepository.save(card);
    }

    @PutMapping("/{cardId}")
    public CardIssues updateCard(@PathVariable int cardId, @RequestBody CardIssues cardDetails) {
        CardIssues card = cardRepository.findById(cardId).orElseThrow(() -> new RuntimeException("Card not found"));
        card.setTitle(cardDetails.getTitle());
        card.setLabel(cardDetails.getLabel());
        card.setDescription(cardDetails.getDescription());
        card.setComments(cardDetails.getComments());
        return cardRepository.save(card);
    }

    @DeleteMapping("/{cardId}")
    public void deleteCard(@PathVariable int cardId) {
        cardRepository.deleteById(cardId);
    }
}
