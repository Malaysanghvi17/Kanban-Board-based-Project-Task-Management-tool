package com.example.kanban_backend.Controller;

import com.example.kanban_backend.Model.Boards;
import com.example.kanban_backend.Model.LaneColumns;
import com.example.kanban_backend.Repository.BoardsRepository;
import com.example.kanban_backend.Repository.LaneColumnsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
// LaneController.java
import java.util.List;

@RestController
@RequestMapping("/api/boards/{boardId}/lanes")
public class LaneColumnsController {

    @Autowired
    private LaneColumnsRepository laneRepository;

    @Autowired
    private BoardsRepository boardRepository;

    @GetMapping
    public List<LaneColumns> getLanesByBoardId(@PathVariable int boardId) {
        Boards board = boardRepository.findById(boardId).orElseThrow(() -> new RuntimeException("Board not found"));
        return board.getLanes();
    }

    @PostMapping
    public LaneColumns createLane(@PathVariable int boardId, @RequestBody LaneColumns lane) {
        Boards board = boardRepository.findById(boardId).orElseThrow(() -> new RuntimeException("Board not found"));
        lane.setBoard(board);
        return laneRepository.save(lane);
    }

    @PutMapping("/{laneId}")
    public LaneColumns updateLane(@PathVariable int laneId, @RequestBody LaneColumns laneDetails) {
        LaneColumns lane = laneRepository.findById(laneId).orElseThrow(() -> new RuntimeException("Lane not found"));
        lane.setTitle(laneDetails.getTitle());
        return laneRepository.save(lane);
    }

    @DeleteMapping("/{laneId}")
    public void deleteLane(@PathVariable int laneId) {
        laneRepository.deleteById(laneId);
    }
}
