package com.purlieus.purlieus.adapters;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.v4.app.FragmentActivity;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.models.BD_Donate;
import com.purlieus.purlieus.models.BD_Seek;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Created by Shaurya on 06-Oct-16.
 */

public class DonorAdapter extends RecyclerView.Adapter<DonorAdapter.UserViewHolder> {

    private List<BD_Seek> list;
    private LayoutInflater inflater;
    private Context context;

    public DonorAdapter(Context context, List<BD_Seek> list) {
        inflater = LayoutInflater.from(context);
        this.list = list;
        this.context = context;

    }

        @Override
    public UserViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View view = inflater.inflate(R.layout.item_seeker, parent, false);

        UserViewHolder holder = new UserViewHolder(view);

    return holder;
    }

    @Override
    public void onBindViewHolder(UserViewHolder holder, int position) {

        BD_Seek model = list.get(position);
        holder.name.setText(model.getName());
        holder.bloodGroup.setText(model.getBloodGroup());
        holder.age.setText(Integer.toString(model.getAge()));
        holder.phoneNum.setText(model.getContactNumber());
        holder.sex.setText(model.getSex());

        if(model.isUrgent()){
            holder.urgentLabel.setVisibility(View.VISIBLE);
        }
        else{
            holder.urgentLabel.setVisibility(View.GONE);
        }

    }

    @Override
    public int getItemCount() {
            return list.size();
            }

    class UserViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {

        TextView name, bloodGroup, phoneNum, sex, age;
        ImageView callButton;
        LinearLayout urgentLabel;


        public UserViewHolder(View itemView) {
            super(itemView);

            name = (TextView) itemView.findViewById(R.id.name_item);
            bloodGroup = (TextView) itemView.findViewById(R.id.blood_group_type_item);
            phoneNum = (TextView) itemView.findViewById(R.id.contact_num_item);
            sex = (TextView) itemView.findViewById(R.id.sex_type_item);
            age = (TextView) itemView.findViewById(R.id.age_value_item);
            urgentLabel = (LinearLayout)itemView.findViewById(R.id.urgent_label);

            callButton = (ImageView) itemView.findViewById(R.id.seeker_call_button);
            callButton.setOnClickListener(this);
        }


        @Override
        public void onClick(View view) {
            String callUri;
            callUri = "tel:" + phoneNum.getText().toString();

            Intent callIntent = new Intent(Intent.ACTION_CALL);
            callIntent.setData(Uri.parse(callUri));

            try {
                context.startActivity(callIntent);
            }catch (SecurityException ex)
            {
                Toast.makeText(context, "Purlieus does not have permission to call", Toast.LENGTH_LONG);
            }
        }
    }
}
