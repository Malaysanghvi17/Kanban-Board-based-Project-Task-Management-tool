package com.example.kanban_backend.Controller;

import com.example.kanban_backend.Model.Boards;
import com.example.kanban_backend.Model.Users;
import com.example.kanban_backend.Repository.BoardsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/users/{uid}/boards")
public class BoardsController {

    @Autowired
    private BoardsRepository boardsRepository;

    @GetMapping
    public ResponseEntity<List<Boards>> getBoards(@PathVariable Integer uid) {
        List<Boards> boards = boardsRepository.findByOwnerUid(uid);
        return ResponseEntity.ok(boards);
    }

    @PostMapping
    public ResponseEntity<Boards> createBoard(@PathVariable Integer uid, @RequestBody Boards newBoard) {
        Users owner = new Users();
        owner.setUid(uid);
        newBoard.setOwner(owner);
        Boards savedBoard = boardsRepository.save(newBoard);
        return ResponseEntity.created(null).body(savedBoard);
    }

    @PutMapping("/{bid}")
    public ResponseEntity<Void> updateBoard(@PathVariable Integer uid, @PathVariable Integer bid, @RequestBody Boards updatedBoard) {
        Boards board = boardsRepository.findById(bid)
                .filter(b -> b.getOwner().getUid().equals(uid))
                .orElse(null);
        if (board == null) return ResponseEntity.notFound().build();

        board.setName(updatedBoard.getName());
        boardsRepository.save(board);
        return ResponseEntity.noContent().build();
    }

    @DeleteMapping("/{bid}")
    public ResponseEntity<Void> deleteBoard(@PathVariable Integer uid, @PathVariable Integer bid) {
        Boards board = boardsRepository.findById(bid)
                .filter(b -> b.getOwner().getUid().equals(uid))
                .orElse(null);
        if (board == null) return ResponseEntity.notFound().build();

        boardsRepository.delete(board);
        return ResponseEntity.noContent().build();
    }
}