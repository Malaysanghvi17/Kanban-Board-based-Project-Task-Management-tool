package com.example.kanban_backend.Repository;
import com.example.kanban_backend.Model.LaneColumns;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface LaneColumnsRepository extends JpaRepository<LaneColumns, Integer> {
    List<LaneColumns> findByBoardBid(Integer bid);
}