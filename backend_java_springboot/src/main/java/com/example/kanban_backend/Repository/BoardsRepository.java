package com.example.kanban_backend.Repository;
import com.example.kanban_backend.Model.Boards;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface BoardsRepository extends JpaRepository<Boards, Integer> {
    List<Boards> findByOwnerUid(Integer uid);
}